# Performance

Guide de performance et optimisations du projet AdvancedDevSample.

---

## Metriques de performance

### Temps de reponse

| Endpoint | Methode | Temps moyen | Cible |
|----------|---------|-------------|-------|
| `/api/auth/login` | POST | ~5ms | < 50ms |
| `/api/products` | GET | ~2ms | < 20ms |
| `/api/products/{id}` | GET | ~1ms | < 10ms |
| `/api/products` | POST | ~3ms | < 30ms |
| `/api/products/{id}/price` | PUT | ~2ms | < 20ms |

> [!NOTE]
> Mesures avec **In-Memory Repository**. Performance DB reelle sera superieure.

---

## Mesurer la performance

### Avec curl

```bash
# Creer curl-format.txt
cat > curl-format.txt << EOF
time_total: %{time_total}s
size_download: %{size_download} bytes
speed_download: %{speed_download} bytes/s
EOF

# Mesurer
curl -w "@curl-format.txt" -o /dev/null -s \
  -H "Authorization: Bearer TOKEN" \
  https://localhost:5001/api/products
```

**Sortie :**

```
time_total: 0.003s
size_download: 245 bytes
speed_download: 81666 bytes/s
```

---

### Avec Apache Bench

```bash
# Installer
brew install httpd  # macOS
sudo apt install apache2-utils  # Linux

# Tester (100 requetes, 10 concurrentes)
ab -n 100 -c 10 \
  -H "Authorization: Bearer TOKEN" \
  https://localhost:5001/api/products
```

**Resultat :**

```
Requests per second:    333.33 [#/sec]
Time per request:       3.000 [ms] (mean)
```

---

## Optimisations appliquees

### 1. In-Memory Repository

**Avantage :** Pas de latence I/O disque.

**Performance :**

- Lecture : **O(1)** pour `GetById` (Dictionary)
- Lecture : **O(n)** pour `GetAll` (iteration)
- Ecriture : **O(1)** pour `Save`

**Code :**

```csharp
private readonly Dictionary<Guid, Product> _products = new();

public Product? GetById(Guid id)
{
    return _products.GetValueOrDefault(id);  // O(1)
}
```

---

### 2. Value Objects immuables

**Avantage :** Pas de copies defensives, thread-safe.

**Code :**

```csharp
public class Price
{
    public decimal Value { get; }  // Immutable
    public string Currency { get; }  // Immutable
}
```

---

### 3. JWT Stateless

**Avantage :** Pas de query DB pour valider le token.

**Performance :**

- Validation : **O(1)** (verification signature)
- Pas de round-trip DB

---

### 4. Middleware d'exception

**Avantage :** Capture centralisee, pas de duplication.

**Impact :** Minimal (< 1ms overhead)

---

## Strategies de caching

> [!WARNING]
> **Non implemente** - Recommandations pour production.

### Response Caching

**Pour endpoints GET immuables :**

```csharp
[HttpGet]
[ResponseCache(Duration = 60)]  // Cache 60s
public IActionResult GetAllProducts()
{
    var products = _service.GetAllProducts();
    return Ok(products);
}
```

---

### Distributed Cache (Redis)

**Pour environment multi-instances :**

```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "AdvancedDevSample_";
});
```

---

### Memory Cache

**Pour donnees frequentes :**

```csharp
private readonly IMemoryCache _cache;

public IEnumerable<ProductDto> GetAllProducts()
{
    return _cache.GetOrCreate("all_products", entry =>
    {
        entry.SlidingExpiration = TimeSpan.FromMinutes(5);
        return _service.GetAllProducts();
    });
}
```

---

## Profiling

### dotnet-trace

```bash
# Installer
dotnet tool install --global dotnet-trace

# Profiler
dotnet-trace collect --process-id <PID>

# Analyser avec PerfView ou speedscope.app
```

---

### BenchmarkDotNet

**Pour micro-benchmarks :**

```csharp
[MemoryDiagnoser]
public class ProductBenchmark
{
    [Benchmark]
    public void CreateProduct()
    {
        var product = new Product("Test", new Price(100m, "EUR"));
    }
}
```

**Run :**

```bash
dotnet run -c Release --project Benchmarks
```

---

## Bottlenecks potentiels

### 1. Entity Framework (si implemente)

**Probleme :** N+1 queries

**Solution :** Eager loading

```csharp
// ❌ Mauvais : N+1
var products = _context.Products.ToList();
foreach (var p in products)
{
    _ = p.Price;  // Query par produit
}

// ✅ Bon : 1 query
var products = _context.Products.Include(p => p.Price).ToList();
```

---

### 2. Serialization JSON

**Probleme :** Serialization lente

**Solution :** System.Text.Json (deja utilise)

**Performance :** ~2x plus rapide que Newtonsoft.Json

---

### 3. Validation

**Probleme :** Validation repetee

**Solution :** Valider une seule fois (Value Objects)

```csharp
// ✅ Bon : validation au constructeur uniquement
public Price(decimal value, string currency)
{
    if (value <= 0)  // Valide 1 seule fois
        throw new DomainException("Le prix doit être positif.");
    
    Value = value;
}
```

---

## Monitoring

### Application Insights (recommande)

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

**Metriques collectees :**

- Temps de reponse par endpoint
- Taux d'erreur
- Dependances (DB, Redis)

---

### Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

app.MapHealthChecks("/health");
```

**Test :**

```bash
curl https://localhost:5001/health
# Response: Healthy
```

---

## Load Testing

### k6 (recommande)

**Script :**

```javascript
import http from 'k6/http';
import { check } from 'k6';

export let options = {
  vus: 100,
  duration: '30s',
};

export default function() {
  let token = 'VOTRE_TOKEN';
  let res = http.get('https://localhost:5001/api/products', {
    headers: { 'Authorization': `Bearer ${token}` },
  });
  
  check(res, {
    'status is 200': (r) => r.status === 200,
    'response time < 50ms': (r) => r.timings.duration < 50,
  });
}
```

**Run :**

```bash
k6 run loadtest.js
```

---

## Optimisations futures

### 1. Output Caching (.NET 10)

```csharp
builder.Services.AddOutputCache();

app.MapGet("/api/products", () => { ... })
   .CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(5)));
```

---

### 2. Ahead-of-Time (AOT) Compilation

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
</PropertyGroup>
```

**Avantages :**

- Startup 4x plus rapide
- Memory footprint reduit

---

### 3. Minimal APIs (deja utilise)

```csharp
// ✅ Bon : Minimal API (rapide)
app.MapPost("/api/auth/login", (LoginRequest req) => { ... });
```

**Performance :** ~15% plus rapide que MVC Controllers

---

## Checklist performance

### Developpement

- [ ] Value Objects immuables
- [ ] Repository O(1) lookup
- [ ] JWT stateless
- [ ] System.Text.Json

### Production

- [ ] Response caching active
- [ ] Distributed cache (Redis)
- [ ] Health checks configures
- [ ] Application Insights active
- [ ] Load testing execute

---

## Ressources

| Outil | Usage | Lien |
|-------|-------|------|
| **dotnet-trace** | Profiling CPU | [Docs](https://learn.microsoft.com/dotnet/core/diagnostics/dotnet-trace) |
| **BenchmarkDotNet** | Micro-benchmarks | [GitHub](https://github.com/dotnet/BenchmarkDotNet) |
| **k6** | Load testing | [k6.io](https://k6.io/) |
| **Application Insights** | Monitoring | [Azure](https://azure.microsoft.com/services/monitor/) |
