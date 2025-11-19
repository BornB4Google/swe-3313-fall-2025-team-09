using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<StorefrontDbContext>(options =>
    options.UseSqlite(connectionString));


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpClient();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StorefrontDbContext>();
    
    db.Database.Migrate();

    // Seed only if empty (first run)
    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            UserId = 1,
            Username = "award62",
            PasswordHash = "a4d2c07604811e53d29e425834f0d1302a519458323773a148f7d23204950e42",
            FirstName = "Amy",
            LastName = "Ward",
            Email = "award62@students.kennesaw.edu",
            IsAdmin = true
        });
    }

    if (!db.InventoryItems.Any())
    {
     db.InventoryItems.AddRange(
    new InventoryItem
    {
        ItemId = 1,
        Name = "Nintendo ADR",
        Description = "Nintendo offers ownership of one of the world’s most influential entertainment portfolios, including Mario, Zelda, Pokémon, and more. Acquire complete control over beloved franchises, future console development, and a global fanbase spanning generations.",
        Price = 17460000000000.00m,
        PrimaryPhotoUrl = "https://cdn.freebiesupply.com/logos/large/2x/nintendo-2-logo-png-transparent.png",
        Category = "Entertainment",
        IsSold = false
    },
    new InventoryItem
    {
        ItemId = 2,
        Name = "LVMH Moët Hennessy Louis Vuitton SE",
        Description = "Take control of the world’s premier luxury empire, from Louis Vuitton to Dior to Moët & Chandon. Prestige, heritage, and global influence in one acquisition.",
        Price = 373600000000.00m,
        PrimaryPhotoUrl = "https://images.seeklogo.com/logo-png/8/1/lvmh-logo-png_seeklogo-86482.png",
        Category = "Luxury Goods",
        IsSold = false
    },
    new InventoryItem
    {
        ItemId = 3,
        Name = "Tesla Automotive",
        Description = "Take command of the world’s most influential EV and energy brand. From autonomous driving breakthroughs to global-scale battery innovation, Tesla offers a powerful blend of technology, mobility, and cultural impact.",
        Price = 1340000000000.00m,
        PrimaryPhotoUrl = "https://storage.googleapis.com/webdesignledger.pub.network/WDL/d2a6d8d2-t3.jpg",
        Category = "Automotive",
        IsSold = false
    },
    new InventoryItem
    {
        ItemId = 4,
        Name = "Walt Disney Co",
        Description = "Own the magic. Purchasing Disney grants control of the entire storytelling empire, including Marvel, Star Wars, Pixar, ESPN, and the global theme park network that defines modern entertainment.",
        Price = 190220000000.00m,
        PrimaryPhotoUrl = "https://images.seeklogo.com/logo-png/50/1/the-walt-disney-company-logo-png_seeklogo-502952.png",
        Category = "Entertainment",
        IsSold = false
    },
    new InventoryItem
    {
        ItemId = 5,
        Name = "Google Deepmind",
        Description = "Own the mind behind the machines. Google DeepMind delivers world-class AI innovation, from advanced neural networks to frontier research, giving you the keys to one of the most powerful technology engines on Earth.",
        Price = 760000000000.00m,
        PrimaryPhotoUrl = "https://media.wired.com/photos/66900a63fc84cb0d65446d72/3:2/w_1920,c_limit/Deepmind-Robotics-Chatbot-Business-2021265856.jpg",
        Category = "Technology",
        IsSold = false
    }
);
    }

    if (!db.ItemImages.Any())
    {
        db.ItemImages.AddRange(
            new ItemImage { ImageId = 1, ItemId = 1, ImageUrl = "https://www.zgf.com/images/2016_04_S21460.00_Nintendo-1500x700.jpg?w=1920", DisplayOrder = 2 },
            new ItemImage { ImageId = 2, ItemId = 1, ImageUrl = "https://assets.nintendo.com/image/upload/w_800,f_auto,q_auto/Play%20Nintendo/Video/posters/play-nintendo", DisplayOrder = 3 },
            new ItemImage { ImageId = 3, ItemId = 2, ImageUrl = "https://a.storyblok.com/f/182663/1344x756/61509b8286/m-a-lvmh-hero-bernard-arnault.png", DisplayOrder = 2 },
            new ItemImage { ImageId = 4, ItemId = 2, ImageUrl = "https://mvcmagazine.com/wp-content/uploads/2021/07/Schermata-2021-07-19-alle-22.52.47.png", DisplayOrder = 3 },
            new ItemImage { ImageId = 5, ItemId = 3, ImageUrl = "https://www.austinchamber.com/uploads/marketing/Blog-Images/tesla_blog.jpg", DisplayOrder = 2 },
            new ItemImage { ImageId = 6, ItemId = 3, ImageUrl = "https://i0.wp.com/electrek.co/wp-content/uploads/sites/3/2025/01/Model-Y-2-Along-the-Way-Tablet-CN.png-e1736950646574.jpeg?w=1500&quality=82&strip=all&ssl=1", DisplayOrder = 3 },
            new ItemImage { ImageId = 7, ItemId = 4, ImageUrl = "https://static.wikia.nocookie.net/disney/images/3/31/Disney2004report.jpg/revision/latest?cb=20140130132003", DisplayOrder = 2 },
            new ItemImage { ImageId = 8, ItemId = 4, ImageUrl = "https://res.cloudinary.com/sagacity/image/upload/c_crop,h_2700,w_1800,x_0,y_0/c_limit,dpr_auto,f_auto,fl_lossy,q_80,w_1080/0725ZN_0276MS_oi24ko.jpg", DisplayOrder = 3 },
            new ItemImage { ImageId = 9, ItemId = 5, ImageUrl = "https://api.time.com/wp-content/uploads/2024/08/GettyImages-2151467843.jpg?quality=85&w=1024", DisplayOrder = 2 },
            new ItemImage { ImageId = 10, ItemId = 5, ImageUrl = "https://static.the-independent.com/2025/03/13/10/40/google-deepmind-robot-gemini.png?quality=75&width=1250&crop=3%3A2%2Csmart&auto=webp", DisplayOrder = 3 }
        );
    }

    db.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// API routes

// DB health check (check for seed data)
app.MapGet("/api/db-health", async (StorefrontDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    var userCount = await db.Users.CountAsync();
    var itemCount = await db.InventoryItems.CountAsync();
    var imageCount = await db.ItemImages.CountAsync();

    return Results.Ok(new
    {
        canConnect,
        userCount,
        itemCount,
        imageCount
    });
});

// Inventory listing
app.MapGet("/api/inventory", async (StorefrontDbContext db) =>
{
    var items = await db.InventoryItems
        .Include(i => i.Images)
        .OrderBy(i => i.ItemId)
        .Select(i => new
        {
            i.ItemId,
            i.Name,
            i.Description,
            i.Price,
            i.PrimaryPhotoUrl,
            i.Category,
            i.IsSold,
            Images = i.Images
                .OrderBy(img => img.DisplayOrder)
                .Select(img => new
                {
                    img.ImageId,
                    img.ImageUrl,
                    img.DisplayOrder
                })
                .ToList()
        })
        .ToListAsync();

    return Results.Ok(items);
});

// Single inventory item by ID (with images)
app.MapGet("/api/inventory/{id:int}", async (int id, StorefrontDbContext db) =>
{
    var item = await db.InventoryItems
        .Include(i => i.Images)
        .Where(i => i.ItemId == id)
        .Select(i => new
        {
            i.ItemId,
            i.Name,
            i.Description,
            i.Price,
            i.PrimaryPhotoUrl,
            i.Category,
            i.IsSold,
            Images = i.Images
                .OrderBy(img => img.DisplayOrder)
                .Select(img => new
                {
                    img.ImageId,
                    img.ImageUrl,
                    img.DisplayOrder
                })
                .ToList()
        })
        .FirstOrDefaultAsync();

    if (item is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(item);
});

if (app.Environment.IsDevelopment())
{
    // Catch static file requests and proxy them too
    app.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value ?? "";
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, path);
        
        // Let API requests through
        if (path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/openapi/", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }
        
        // Proxy everything else to Angular
        var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();
        var angularDevServer = "http://localhost:4200";
        var targetUri = $"{angularDevServer}{path}{context.Request.QueryString}";
        
        logger.LogInformation("Proxying to: {TargetUri}", targetUri);
        
        try
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(context.Request.Method),
                RequestUri = new Uri(targetUri)
            };
            
            foreach (var header in context.Request.Headers)
            {
                if (!header.Key.StartsWith(":", StringComparison.OrdinalIgnoreCase))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
            
            var responseMessage = await httpClient.SendAsync(requestMessage, 
                HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
            
            context.Response.StatusCode = (int)responseMessage.StatusCode;
            
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            
            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            
            context.Response.Headers.Remove("transfer-encoding");
            
            await responseMessage.Content.CopyToAsync(context.Response.Body);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error proxying request");
            context.Response.StatusCode = 502;
            await context.Response.WriteAsync("Angular dev server not running");
        }
    });
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");
}

app.Run();

