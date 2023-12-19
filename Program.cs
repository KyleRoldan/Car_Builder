using CarBuilder.Models;
using CarBuilder.Models.DTOs;





List<PaintColor> paintColors = new List<PaintColor>
{
           new PaintColor
           {
              Id = 1,
              Price = 4.9M,
              Color = "Silver"
           },


           new PaintColor
           {
              Id = 2,
              Price = 1.5M,
              Color = "Midnight Blue"
           },


           new PaintColor
           {
              Id = 3,
              Price = 1.4M,
              Color = "Firebrick Red"
           },


            new PaintColor
           {
              Id = 4,
              Price = 1.4M,
              Color = "Spring Green"
           }
};


List<Interior> interiors = new List<Interior>
{
           new Interior
           {
               Id = 1,
               Price = 1.5M,
               Material = "Beige Fabric"


           },


           new Interior
           {
               Id = 2,
               Price = 2.5M,
               Material = "Charcoal Fabric"




           },
           new Interior
           {
               Id = 3,
               Price = 3.5M,
               Material = "White Leather"


           },
           new Interior
           {
               Id = 4,
               Price = 4.5M,
               Material = "Black Leather"


           },
};


List<Order> orders = new List<Order>


{
            new Order
            {
                Id = 1,
                Timestamp = DateTime.MinValue,
                WheelId = 3,
                TechnologyId = 0,
                PaintId = 0,
                InteriorId = 0,
            },


            new Order
            {
               Id = 2,
                Timestamp = DateTime.MinValue,
                WheelId = 3,
                TechnologyId = 1,
                PaintId = 1,
                InteriorId = 1,
            },


            new Order
            {
                Id = 3,
                Timestamp = DateTime.MinValue,
                WheelId = 3,
                TechnologyId = 2,
                PaintId = 2,
                InteriorId = 2,       
            },


            new Order
            {
               Id = 4,
                Timestamp = DateTime.MinValue,
                WheelId = 3,
                TechnologyId = 2,
                PaintId = 1,
                InteriorId = 3,
            },


            new Order
            {
               Id = 5,
                Timestamp = DateTime.MinValue,
                WheelId = 3,
                TechnologyId = 2,
                PaintId = 1,
                InteriorId = 3,
            }
};


List<Technology> technologies = new List<Technology>
{
           new Technology
           {
               Id = 1,
               Price = 1.5M,
               Package = "Basic Package (basic sound system)"


           },


           new Technology
           {
               Id = 2,
               Price = 2.5M,
               Package = "Navigation Package (includes integrated navigation controls)"




           },
           new Technology
           {
               Id = 3,
               Price = 3.5M,
               Package = "Visibility Package (includes side and rear cameras)"


           },
           new Technology
           {
               Id = 4,
               Price = 4.5M,
               Package = "Ultra Package (includes navigation and visibility packages)"
           },
};


List<Wheels> wheels = new List<Wheels>
{
           new Wheels
           {
               Id = 1,
               Price = 1.5M,
               Style = "17-inch Pair Radial"


           },


           new Wheels
           {
               Id = 2,
               Price = 2.5M,
               Style= "17-inch Pair Radial Black"




           },
           new Wheels
           {
               Id = 3,
               Price = 3.5M,
               Style = "18-inch Pair Spoke Silver"


           },
           new Wheels
           {
               Id = 4,
               Price = 4.5M,
               Style = "8-inch Pair Spoke Black"
           },
};


///////////////////////DONT TOUCH THIS CODE//////////////////////////////////////////////////////////////


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
}



app.UseHttpsRedirection();
//////////////////////////////////////////////////////////////////////////////////////////////////////////////




/////////////////Interior Endpoint/////////////////////////////////////////////////////////////////////////////
app.MapGet("/interiors", () =>
{
   return interiors.Select(i => new InteriorDTO
   {
       Id = i.Id,
       Price = i.Price,
       Material = i.Material
   });
});


/////////////////Paint Color Endpoint/////////////////////////////////////////////////////////////////////////////
app.MapGet("/paintcolors", () =>
{
   return paintColors.Select(p => new PaintColorDTO
   {
       Id = p.Id,
       Price = p.Price,
       Color = p.Color
   });
});


/////////////////Technology Endpoint/////////////////////////////////////////////////////////////////////////////
app.MapGet("/technology", () =>
{
   return technologies.Select(t => new TechnologyDTO
   {
       Id = t.Id,
       Price = t.Price,
       Package = t.Package
   });
});


/////////////////Wheels Endpoint/////////////////////////////////////////////////////////////////////////////
app.MapGet("/wheels", () =>
{
   return wheels.Select(w => new WheelsDTO
   {
       Id = w.Id,
       Price = w.Price,
       Style = w.Style
   });
});

/////////////////Orders/////////////////////////////////////////////////////////////////////////////

app.MapGet("/orders", () =>
{
    

    var fulfilledOrderDTOs = orders.Where(o => !o.IsFulfilled).Select(o =>
    {
        Wheels wheel = wheels.FirstOrDefault(w => w.Id == o.WheelId);
        Technology technology = technologies.FirstOrDefault(t => t.Id == o.TechnologyId);
        PaintColor paintColor = paintColors.FirstOrDefault(p => p.Id == o.PaintId);
        Interior interior = interiors.FirstOrDefault(i => i.Id == o.InteriorId);
        
        

        return new OrderDTO
        {
        Id = o.Id,
        Timestamp = o.Timestamp,
        WheelId = wheels.FirstOrDefault(w => w.Id == o.WheelId)?.Id ?? 0,
        Wheels = wheel == null ? null : new WheelsDTO
        {
            Id = wheel.Id,
            Price = wheel.Price,
            Style = wheel.Style
        },
        TechnologyId = technologies.FirstOrDefault(t => t.Id == o.TechnologyId)?.Id ?? 0,
        Technology = technology == null ? null : new TechnologyDTO
        {
            Id = technology.Id,
            Price = technology.Price,
            Package = technology.Package
        }, 
        PaintId = paintColors.FirstOrDefault(p => p.Id == o.PaintId)?.Id ?? 0,
        PaintColor = paintColor == null ? null : new PaintColorDTO
        {
            Id = paintColor.Id,
            Price = paintColor.Price,
            Color = paintColor.Color
        }, 
        InteriorId = interiors.FirstOrDefault(i => i.Id == o.InteriorId)?.Id ?? 0,
        Interior = interior == null ? null : new InteriorDTO
        {
            Id = interior.Id,
            Price = interior.Price,
            Material = interior.Material
        }, 
         IsFulfilled = o.IsFulfilled
        };
    });

    return fulfilledOrderDTOs;
});
////////////////LONGER WAY TO DO ORDER ENDPOINT///////////////////////////////////////////////////////////////////////
// app.MapGet("/orders", () =>
// {
//     var orderDTOs = orders.Select(o =>
//     {
//         var wheel = wheels.FirstOrDefault(w => w.Id == o.WheelId);
//         var technology = technologies.FirstOrDefault(t => t.Id == o.TechnologyId);
//         var paintColor = paintColors.FirstOrDefault(p => p.Id == o.PaintId);
//         var interior = interiors.FirstOrDefault(i => i.Id == o.InteriorId);

//         if (wheel == null || technology == null || paintColor == null || interior == null)
//         {
//             return Results.NotFound();
//         }

//         return Results.Ok(new OrderDTO
//         {
//             Id = o.Id,
//             Timestamp = o.Timestamp,
//             WheelId = wheel.Id,
//             Wheel = wheel.Style,
//             TechnologyId = technology.Id,
//             Technology = technology.Package,
//             PaintId = paintColor.Id,
//             PaintColor = paintColor.Color,
//             InteriorId = interior.Id,
//             Interior = interior.Material
//         });
//     });

//     return orderDTOs;
// });

///////////////////////Create an Order//////////////////////////////////////////////////////////////////////////////////////////

app.MapPost("/orders", (Order order) =>
{


    // creates a new id 
    order.Id = orders.Max(or => or.Id) + 1;
    orders.Add(order);

    // Created returns a 201 status code with a link in the headers to where the new resource can be accessed
    return Results.Created($"/orders/{order.Id}", new OrderDTO
    {
        Id = order.Id, 
        Timestamp = DateTime.Today,
        WheelId = order.WheelId,
        TechnologyId = order.TechnologyId, 
        PaintId = order.PaintId,
        InteriorId = order.TechnologyId,
        IsFulfilled = order.IsFulfilled
      
        
    });

});

/////////////////////// MARK AN ORDER AS FULFILLED/////////////////////////////////////////////////////////

app.MapPost("/orders/{Id}/fulfill", (int Id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == Id);

    if (order == null)
    {
        return Results.NotFound();
    }

    order.IsFulfilled = true;

    return Results.Ok();
});



app.Run();

