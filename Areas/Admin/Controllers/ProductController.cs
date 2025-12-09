
using Backend_MVC_TASK_1.DAL;
using Backend_MVC_TASK_1.Models;
using Backend_MVC_TASK_1.Utilities.Enums;
using Backend_MVC_TASK_1.Utilities.Extensions;
using Backend_MVC_TASK_1.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Pronia.ViewModels;
using System.Drawing;

    namespace Pronia.Areas.Admin.Controllers
    { 
        [Area("Admin")]
        public class ProductController : Controller
        {
            private readonly AppDbContext _context;
            private readonly IWebHostEnvironment _env;

            public ProductController(AppDbContext context, IWebHostEnvironment env)
            {
                _context = context;
                _env = env;
            }
            public async Task<IActionResult> Index()
            {
            var productVMs = await _context.Products
                    .Select(p => new GetAdminProductVM
                    {
                        Name = p.Name,
                        Id = p.Id,
                        Price = p.Price,
                        Image = p.ProductImages.FirstOrDefault(pi => pi.IsPrimaryImage == true).Image,
                        CategoryName = p.Category.Name
                    })
                    .ToListAsync();

                return View(productVMs);
            }

            public async Task<IActionResult> Create()
            {
                CreateProductVM productVM = new()
                {
                    Categories = await _context.Categories.ToListAsync(),
                    Tags = await _context.Tags.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Sizes = await _context.Sizes.ToListAsync()
                };
                return View(productVM);
            }
            [HttpPost]
            public async Task<IActionResult> Create(CreateProductVM productVM)
            {

                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Tags = await _context.Tags.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                productVM.Sizes = await _context.Sizes.ToListAsync();

                if (!ModelState.IsValid)
                {

                    return View(productVM);
                }

                if (!productVM.PrimaryPhoto.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(CreateProductVM.PrimaryPhoto), "File type is incorrect");
                    return View(productVM);
                }
                if (!productVM.PrimaryPhoto.ValidateSize(FileSize.MB, 1))
                {
                    ModelState.AddModelError(nameof(CreateProductVM.PrimaryPhoto), "File size is incorrect");
                    return View(productVM);
                }
                if (!productVM.SecondaryPhoto.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(CreateProductVM.SecondaryPhoto), "File type is incorrect");
                    return View(productVM);
                }
                if (!productVM.SecondaryPhoto.ValidateSize(FileSize.MB, 1))
                {
                    ModelState.AddModelError(nameof(CreateProductVM.SecondaryPhoto), "File size is incorrect");
                    return View(productVM);
                }


                bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
                if (!result)
                {
                    ModelState.AddModelError(nameof(CreateProductVM.CategoryId), "Category does not exists");
                    return View(productVM);
                }
                if (productVM.TagIds is null)
                {
                    productVM.TagIds = new();
                }
                if (productVM.ColorIds is null)
                {
                    productVM.ColorIds = new();
                }
                if (productVM.SizeIds is null)
                {
                    productVM.SizeIds = new();
                }
                productVM.TagIds = productVM.TagIds.Distinct().ToList();

                bool tagResult = productVM.TagIds.Any(tId => !productVM.Tags.Exists(t => t.Id == tId));
                if (tagResult)
                {
                    ModelState.AddModelError(nameof(CreateProductVM.TagIds), "Tags are wrong");
                    return View(productVM);
                }




                bool colorResult = productVM.ColorIds.Any(cId => !productVM.Colors.Exists(c => c.Id == cId));
                if (colorResult)
                {
                    ModelState.AddModelError(nameof(CreateProductVM.ColorIds), "Color are wrong");
                    return View(productVM);
                }




                bool sizeResult = productVM.SizeIds.Any(sId => !productVM.Sizes.Exists(s => s.Id == sId));

                if (sizeResult)
                {
                    ModelState.AddModelError(nameof(CreateProductVM.SizeIds), "Size are wrong");
                    return View(productVM);
                }


                bool resultName = await _context.Products.AnyAsync(p => p.Name == productVM.Name);
                if (resultName)
                {
                    ModelState.AddModelError(nameof(CreateProductVM.Name), "Product name already exists");
                    return View(productVM);
                }


                ProductImage main = new ProductImage
                {
                    Image = await productVM.PrimaryPhoto.CreateFileAysnc(_env.WebRootPath, "assets", "images", "website-images"),
                    IsPrimaryImage = true,
                    CreatedAt = DateTime.Now
                };
                ProductImage secondary = new ProductImage
                {
                    Image = await productVM.SecondaryPhoto.CreateFileAysnc(_env.WebRootPath, "assets", "images", "website-images"),
                    IsPrimaryImage = false,
                    CreatedAt = DateTime.Now
                };




                Product product = new()
                {
                    Name = productVM.Name,
                    SKU = productVM.SKU,
                    Price = productVM.Price.Value,
                    Description = productVM.Description,
                    CategoryId = productVM.CategoryId.Value,
                    CreatedAt = DateTime.Now,
                    ProductTags = productVM.TagIds.Select(tId => new ProductTag { TagId = tId }).ToList(),
                    ProductColors = productVM.ColorIds.Select(cId => new ProductColor { ColorId = cId }).ToList(),
                    ProductSizes = productVM.SizeIds.Select(sId => new ProductSize { SizeId = sId }).ToList(),
                    ProductImages = new()
                {
                    main,
                    secondary
                }

                };
                string message = string.Empty;

                foreach (IFormFile file in productVM.AdditionalPhotos)
                {

                    if (!file.ValidateType("image/"))
                    {

                        message += $"<p class=\"text-warning\">{file.Name} file type is inccorect</p>";
                        continue;
                    }
                    if (!file.ValidateSize(FileSize.MB, 1))
                    {
                        message += $"<p class=\"text-warning\">{file.Name} file size is inccorect</p>";
                        continue;
                    }


                    product.ProductImages.Add(new()
                    {
                        Image = await file.CreateFileAysnc(_env.WebRootPath, "assets", "images", "website-images"),
                        IsPrimaryImage = null,
                        CreatedAt = DateTime.Now

                    });
                }

                TempData["ImageWarning"] = message;





                _context.Add(product);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            public async Task<IActionResult> Update(int? id)
            {
                if (id is null || id < 1)
                {
                    return BadRequest();
                }
                Product? existed = await _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductTags)
                    .Include(p => p.ProductColors)
                    .Include(p => p.ProductSizes)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (existed is null)
                {
                    return NotFound();
                }

                UpdateProductVM productVM = new()
                {
                    Name = existed.Name,
                    SKU = existed.SKU,
                    Description = existed.Description,
                    CategoryId = existed.CategoryId,
                    Price = existed.Price,
                    TagIds = existed.ProductTags.Select(pt => pt.TagId).ToList(),
                    ColorIds = existed.ProductColors.Select(pc => pc.ColorId).ToList(),
                    SizeIds = existed.ProductSizes.Select(ps => ps.SizeId).ToList(),

                    Categories = await _context.Categories.ToListAsync(),
                    Tags = await _context.Tags.ToListAsync(),
                    Colors = await _context.Colors.ToListAsync(),
                    Sizes = await _context.Sizes.ToListAsync(),
                    ProductImages = existed.ProductImages
                };
                return View(productVM);
            }

            [HttpPost]
            public async Task<IActionResult> Update(int? id, UpdateProductVM productVM)
            {
                Product? existed = await _context.Products
                  .Include(p => p.ProductImages)
                  .Include(p => p.ProductTags)
                  .Include(p => p.ProductColors)
                  .Include(p => p.ProductSizes)
                  .Include(p => p.Category)
                  .FirstOrDefaultAsync(p => p.Id == id);

                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Tags = await _context.Tags.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                productVM.Sizes = await _context.Sizes.ToListAsync();
                productVM.ProductImages = existed.ProductImages;

                if (!ModelState.IsValid)
                {
                    return View(productVM);
                }
                if (productVM.PrimaryPhoto is not null)
                {

                    if (!productVM.PrimaryPhoto.ValidateType("image/"))
                    {
                        ModelState.AddModelError(nameof(UpdateProductVM.PrimaryPhoto), "File type is incorrect");
                        return View(productVM);
                    }
                    if (!productVM.PrimaryPhoto.ValidateSize(FileSize.MB, 1))
                    {
                        ModelState.AddModelError(nameof(UpdateProductVM.PrimaryPhoto), "File size is incorrect");
                        return View(productVM);
                    }
                }
                if (productVM.SecondaryPhoto is not null)
                {

                    if (!productVM.SecondaryPhoto.ValidateType("image/"))
                    {
                        ModelState.AddModelError(nameof(UpdateProductVM.SecondaryPhoto), "File type is incorrect");
                        return View(productVM);
                    }
                    if (!productVM.SecondaryPhoto.ValidateSize(FileSize.MB, 1))
                    {
                        ModelState.AddModelError(nameof(UpdateProductVM.SecondaryPhoto), "File size is incorrect");
                        return View(productVM);
                    }
                }
                bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
                if (!result)
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.CategoryId), "Category does not exists");
                    return View(productVM);
                }
                if (productVM.TagIds is null)
                {
                    productVM.TagIds = new();
                }

                productVM.TagIds = productVM.TagIds.Distinct().ToList();

                bool tagResult = productVM.TagIds.Any(tId => !productVM.Tags.Exists(t => t.Id == tId));
                if (tagResult)
                {
                    ModelState.AddModelError(nameof(CreateProductVM.TagIds), "Tags are wrong");
                    return View(productVM);
                }
                productVM.ColorIds = productVM.ColorIds.Distinct().ToList();
                bool colorResult = productVM.ColorIds.Any(cId => !productVM.Colors.Exists(c => c.Id == cId));
                if (tagResult)
                {
                    ModelState.AddModelError(nameof(CreateProductVM.TagIds), "Colors are wrong");
                    return View(productVM);
                }
                productVM.SizeIds = productVM.SizeIds.Distinct().ToList();
                bool sizeResult = productVM.SizeIds.Any(sId => !productVM.Sizes.Exists(s => s.Id == sId));

                bool resultName = await _context.Products.AnyAsync(p => p.Name == productVM.Name && p.Id != id);
                if (resultName)
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.Name), "Product name already exists");
                    return View(productVM);
                }


                if (productVM.PrimaryPhoto is not null)
                {
                    string mainFileName = await productVM.PrimaryPhoto.CreateFileAysnc(_env.WebRootPath, "assets", "images", "website-images");
                    ProductImage existedMain = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimaryImage == true);
                    existedMain.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");

                    existed.ProductImages.Remove(existedMain);
                    existed.ProductImages.Add(new()
                    {
                        Image = mainFileName,
                        IsPrimaryImage = true,
                        CreatedAt = DateTime.Now

                    });
                }
                if (productVM.SecondaryPhoto is not null)
                {
                    string secondaryFileName = await productVM.SecondaryPhoto.CreateFileAysnc(_env.WebRootPath, "assets", "images", "website-images");
                    ProductImage existedSecondary = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimaryImage == false);
                    existedSecondary.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");

                    existed.ProductImages.Remove(existedSecondary);
                    existed.ProductImages.Add(new()
                    {
                        Image = secondaryFileName,
                        IsPrimaryImage = false,
                        CreatedAt = DateTime.Now

                    });
                }
                if (productVM.ImageIds is null)
                {
                    productVM.ImageIds = new();
                }
                List<ProductImage> deleteImages = existed.ProductImages
                     .Where(pi => !productVM.ImageIds
                         .Exists(imgId => pi.Id == imgId) && pi.IsPrimaryImage == null)
                     .ToList();

                deleteImages
                    .ForEach(di => di.Image
                        .DeleteFile(_env.WebRootPath, "assets", "images", "website-images"));
                _context.ProductImages.RemoveRange(deleteImages);

               
                existed.ProductTags = new List<ProductTag>();

                productVM.TagIds.ForEach(tId =>
                {
                    existed.ProductTags.Add(new ProductTag()
                    {
                        ProductId = existed.Id,
                        TagId = tId
                    });
                });
                existed.ProductColors = new List<ProductColor>();
                productVM.ColorIds.ForEach(cId =>
                {
                    existed.ProductColors.Add(new ProductColor()
                    {
                        ProductId = existed.Id,
                        ColorId = cId
                    });
                });
                existed.ProductSizes = new List<ProductSize>();
                productVM.SizeIds.ForEach(sId =>
                {
                    existed.ProductSizes.Add(new ProductSize()
                    {
                        ProductId = existed.Id,
                        SizeId = sId
                    });
                });
                if (productVM.AdditionalPhotos is not null)
                {
                    string message = string.Empty;

                    foreach (IFormFile file in productVM.AdditionalPhotos)
                    {

                        if (!file.ValidateType("image/"))
                        {

                            message += $"<p class=\"text-warning\">{file.Name} file type is inccorect</p>";
                            continue;
                        }
                        if (!file.ValidateSize(FileSize.MB, 1))
                        {
                            message += $"<p class=\"text-warning\">{file.Name} file size is inccorect</p>";
                            continue;
                        }


                        existed.ProductImages.Add(new ProductImage()
                        {
                            Image = await file.CreateFileAysnc(_env.WebRootPath, "assets", "images", "website-images"),
                            IsPrimaryImage = null,
                            CreatedAt = DateTime.Now

                        });
                    }

                    TempData["ImageWarning"] = message;

                }




                existed.Name = productVM.Name;
                existed.SKU = productVM.SKU;
                existed.Description = productVM.Description;
                existed.Price = productVM.Price.Value;
                existed.CategoryId = productVM.CategoryId.Value;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            public async Task<IActionResult> Delete(int? id)
            {
                if (id is null || id < 1)
                {
                    return BadRequest();
                }
                Product? product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (product is null)
                {
                    return NotFound();
                }
                product.ProductImages
                    .ForEach(pi => pi.Image
                        .DeleteFile(_env.WebRootPath, "assets", "images", "website-images"));

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }





        }
    }
    


