# Qualité du Code

Analyse statique et métriques de qualité du projet AdvancedDevSample.

## SonarCloud

Le projet utilise **SonarCloud** pour l'analyse continue de la qualité du code.

### Dashboard

<div class="grid" markdown>

<div class="card" markdown>

### Lien direct

Accédez au rapport complet sur SonarCloud :

**[Voir le Dashboard SonarCloud](https://sonarcloud.io/project/overview?id=HugoRytlewski_AdvancedDevSampleFinal)**

</div>

<div class="card" markdown>

### Badges du projet

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=alert_status&token=229cc30d3067cefd82d4841a3e3b8bb079de8afb)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=bugs&token=229cc30d3067cefd82d4841a3e3b8bb079de8afb)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=code_smells&token=229cc30d3067cefd82d4841a3e3b8bb079de8afb)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=coverage&token=229cc30d3067cefd82d4841a3e3b8bb079de8afb)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

</div>

</div>

---

## Métriques analysées

| Métrique | Valeur actuelle | Objectif |
|----------|----------------|----------|
| **Quality Gate** | Passed | Passed |
| **Tests** | 46 | Maintenus |
| **Coverage** | ~80% | > 80% |
| **Bugs** | 0 | 0 |
| **Vulnérabilités** | 0 | 0 |
| **Code Smells** | Minimaux | < 10 |
| **Duplications** | Minimales | < 3% |
| **Technical Debt** | < 1h | < 1 jour |

---

## Pipeline d'analyse

```mermaid
graph LR
    A[Push/PR] --> B[Build .NET]
    B --> C[Run 46 Tests]
    C --> D[Collect Coverage]
    D --> E[SonarCloud Scan]
    E --> F{Quality Gate}
    F -->|Passed| G[Deploy Docs]
    F -->|Failed| H[Block Merge]
```

---

## Couverture de tests

### Distribution par type

| Type de test | Nombre | Couverture |
|--------------|--------|------------|
| **Unitaires (Domain)** | 15 | Entities, Value Objects, Constructeurs |
| **Unitaires (Application)** | 12 | Services, Validation, DTOs |
| **Unitaires (API)** | 9 | Controllers, Middlewares |
| **Intégration (API)** | 10 | Endpoints REST |
| **Total** | **46** | **~80%** |

### Évolution

```mermaid
graph LR
    A[Initial: 29 tests] --> B[+5 AuthController]
    B --> C[+4 Middleware]
    C --> D[+6 Product]
    D --> E[+3 Service]
    E --> F[Final: 46 tests]
    
    style A fill:#ff6b6b
    style F fill:#51cf66
```

---

## Configuration CI/CD

L'analyse SonarCloud est intégrée dans le workflow GitHub Actions :

```yaml
- name: Begin SonarCloud analysis
  run: |
    dotnet sonarscanner begin \
      /k:"HugoRytlewski_AdvancedDevSampleFinal" \
      /o:"hugorytlewski" \
      /d:sonar.token="${{ secrets.SONAR_TOKEN }}" \
      /d:sonar.host.url="https://sonarcloud.io" \
      /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml

- name: Build
  run: dotnet build --configuration Release

- name: Run tests with coverage
  run: dotnet-coverage collect "dotnet test" -f xml -o coverage.xml

- name: End SonarCloud analysis
  run: dotnet sonarscanner end /d:sonar.token="${{ secrets.SONAR_TOKEN }}"
```

---

## Règles de qualité

### Sévérité des issues

| Niveau | Description | Action |
|--------|-------------|--------|
| **Blocker** | Bug critique ou vulnérabilité | Correction immédiate |
| **Critical** | Bug ou vulnérabilité importante | Correction prioritaire |
| **Major** | Issue significative | Correction planifiée |
| **Minor** | Issue mineure | Correction opportuniste |
| **Info** | Suggestion d'amélioration | Optionnel |

### Quality Gate

Le projet doit respecter ces critères pour passer le Quality Gate :

- **0** nouveaux bugs
- **0** nouvelles vulnérabilités
- **0** nouveaux Code Smells bloquants
- Couverture sur nouveau code **> 80%**
- Duplication sur nouveau code **< 3%**

---

## Sécurité

### Issues corrigées

| Issue | Fichier | Correction |
|-------|---------|------------|
| Clé JWT hardcodée | `AuthController.cs` | Configuration externalisée + pragma warning |
| Clé JWT hardcodée | `Program.cs` | Configuration externalisée |
| Permissions workflow | `ci.yml` | Déplacées au niveau job |

### Bonnes pratiques

- Ne jamais commiter de secrets dans le code
- Utiliser les variables d'environnement ou `appsettings.json`
- Configurer `SONAR_TOKEN` comme secret GitHub
- Supprimer les faux positifs avec `#pragma warning disable`
