# etsy-ai-generator
Generative AI for Etsy Sellers. Effortlessly create compelling, SEO-friendly listings. Spend less time typing and more time making products your customers will love.

## Solution layout

| Path | Description |
| --- | --- |
| `src/EtsyAiGenerator.Api` | ASP.NET Core Web API host configured with controllers, health checks, and production-ready middleware. |
| `src/EtsyAiGenerator.Application` | Application layer that owns the Listing Builder contracts and placeholder services. |
| `tests/EtsyAiGenerator.Application.Tests` | xUnit test suite covering the Listing Builder service behavior. |

## Getting started

1. Install the .NET SDK 8.0 or newer.
2. Restore dependencies:
	```powershell
	dotnet restore src/EtsyAiGenerator.Api/EtsyAiGenerator.Api.csproj
	```
3. Run the local API:
	```powershell
	dotnet run --project src/EtsyAiGenerator.Api/EtsyAiGenerator.Api.csproj
	```
4. Navigate to `https://localhost:7084/health` (or the HTTP port shown in the console) to verify the service is alive.

## Listing Builder placeholder API

`POST /api/listing-builder/preview` accepts a listing draft payload and returns deterministic copy/tags. Example request:

```http
POST https://localhost:7084/api/listing-builder/preview
Content-Type: application/json

{
	"title": "Handmade Ceramic Mug",
	"productType": "Mug",
	"targetCustomer": "coffee lovers",
	"tone": "Cheerful",
	"materials": ["Stoneware", "Lead-free glaze"],
	"highlights": ["Dishwasher safe", "12oz capacity"],
	"price": 34.95
}
```

Use this stub output while wiring up real AI providers and downstream persistence.

## Health checks

- `GET /health` &mdash; a branded HTML status page for quick manual verification.
- `GET /health/status` &mdash; machine-friendly JSON produced by the built-in ASP.NET Core health checks, suitable for load balancers and uptime monitors.

Example CLI invocation:

```powershell
curl https://localhost:7084/health/status --insecure
```

Both endpoints are available as soon as you start the web app, making it easy to plug the service into deployment pipelines or monitoring tools.

## Testing

Run the test suite (currently targeting the application layer):

```powershell
dotnet test etsy-ai-generator.sln
```

Add additional test projects as new capabilities (domain logic, infrastructure, etc.) come online.
