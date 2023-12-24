using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Füge Dienste zum Container hinzu.
builder.Services.AddControllers();

// Konfiguriere die API-Dokumentation mit Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();

// Lade eine optionale Konfigurationsdatei für Geheimnisse, die automatisch neu geladen wird, wenn sie geändert wird.
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

// Konfiguriere SwaggerGen mit OAuth2-Sicherheitsdefinitionen.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    // Füge Filter für Sicherheitsanforderungen hinzu.
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Füge JWT-Authentifizierung hinzu und konfiguriere Validierungsoptionen.
builder.Services.AddAuthentication().AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            // Token-Signaturschlüssel validieren.
            ValidateIssuerSigningKey = true,
            // Publikum (Audience) nicht validieren.
            ValidateAudience = false,
            // Herausgeber (Issuer) nicht validieren.
            ValidateIssuer = false,
            // Symmetrischen Schlüssel für die Signatur validieren.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"]))
        };
    }
);

var app = builder.Build();

// Konfiguriere den HTTP-Anforderungs-Pipeline.
if (app.Environment.IsDevelopment())
{
    // Verwende Swagger in der Entwicklungsphase für API-Dokumentation.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Benutze die Autorisierungsmiddleware.
app.UseAuthorization();

// Karte die Controller-Endpunkte.
app.MapControllers();

// Starte die Anwendung.
app.Run();
