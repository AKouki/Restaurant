# Restaurant

A fully featured website for a restaurant, developed in .NET 7. It consists of two parts, the front-end and the admin panel. The front-end is the presentation website for the customers. The admin panel allows you to manage the reservations and the content of site.

Live Demo: [Front-end](https://demo4-kouki.azurewebsites.net/) / [Admin Panel](https://demo4-kouki.azurewebsites.net/Admin)

## Features

#### Front-end
- Online reservation form
- Contact form
- Google reCAPTCHA v3
- Fully SEO-optimized
- Fully responsive design
- Cross-browser compatible
- Multilingual (Currently Greek and English)

#### Admin Panel
- Reservations management
- Front site content management (Menu/Recommendations/Reviews)
- Ability to reorder menu, menu items, and recommendations with drag and drop
- Dashboard with reservation details
- Send confirmation emails with the booking details

## Technologies
- [ASP.NET Core 7](https://github.com/dotnet/aspnetcore)
- [Entity Framework Core 7](https://github.com/dotnet/efcore)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [xUnit](https://github.com/xunit/xunit)
- [SendGrid](https://github.com/sendgrid/sendgrid-csharp/)

Additional libraries used in front-end: [lightbox2](https://github.com/lokesh/lightbox2), [OwlCarousel2](https://github.com/OwlCarousel2/OwlCarousel2), [Bootstrap 5](https://github.com/twbs/bootstrap), [jQuery](https://github.com/jquery/jquery).

## Getting Started
First you will need to get an API key for the Google reCAPTCHA v3 and the SendGrid.

To run the application:

1. Get the code. You can fork the repository or download it locally.
2. Open the project with Visual Studio
3. Resolve all the nuget packages
4. Update the below part in `appsettings.json` with your Google reCAPTCHA v3 and SendGrid details:

    ```json
      "SendGridOptions": {
        "ApiKey": "",
        "FromEmail": "",
        "FromName": ""
      },
      "ReCaptcha": {
        "SiteKey": "",
        "SecretKey": "",
        "BaseUrl": "https://www.google.com/recaptcha/api/siteverify"
      },
    ```

### Create the database

1. In Visual Studio, open the `Package Manager Console`
2. Run the command: `update-database`

### Run the project
You can run the project via Visual Studio or you can navigate to the project folder and run the command: `dotnet run`
