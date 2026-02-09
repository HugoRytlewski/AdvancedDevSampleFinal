# Qualite du Code

Analyse statique et metriques de qualite du projet AdvancedDevSample.

## SonarCloud

Le projet utilise **SonarCloud** pour l'analyse continue de la qualite du code.

### Dashboard

<div class="grid" markdown>

<div class="card" markdown>

### Lien direct

Accedez au rapport complet sur SonarCloud :

**[Voir le Dashboard SonarCloud](https://sonarcloud.io/project/overview?id=HugoRytlewski_AdvancedDevSampleFinal)**

</div>

<div class="card" markdown>

### Badges du projet

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=bugs)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=coverage)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

</div>

</div>

---

## Metriques analysees

| Metrique | Description | Objectif |
|----------|-------------|----------|
| **Quality Gate** | Evaluation globale du projet | Passed |
| **Bugs** | Erreurs potentielles dans le code | 0 |
| **Vulnerabilities** | Failles de securite | 0 |
| **Code Smells** | Problemes de maintenabilite | Minimiser |
| **Coverage** | Couverture des tests | > 80% |
| **Duplications** | Code duplique | < 3% |
| **Technical Debt** | Temps estime pour corriger les issues | < 1 jour |

---

## Pipeline d'analyse

```mermaid
graph LR
    A[Push/PR] --> B[Build .NET]
    B --> C[Run Tests]
    C --> D[Collect Coverage]
    D --> E[SonarCloud Scan]
    E --> F{Quality Gate}
    F -->|Passed| G[Deploy Docs]
    F -->|Failed| H[Block Merge]
```

---

## Configuration CI/CD

L'analyse SonarCloud est integree dans le workflow GitHub Actions :

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

## Regles de qualite

### Severite des issues

| Niveau | Description | Action |
|--------|-------------|--------|
| **Blocker** | Bug critique ou vulnerabilite | Correction immediate |
| **Critical** | Bug ou vulnerabilite importante | Correction prioritaire |
| **Major** | Issue significative | Correction planifiee |
| **Minor** | Issue mineure | Correction opportuniste |
| **Info** | Suggestion d'amelioration | Optionnel |

### Quality Gate

Le projet doit respecter ces criteres pour passer le Quality Gate :

- **0** nouveaux bugs
- **0** nouvelles vulnerabilites
- **0** nouveaux Code Smells bloquants
- Couverture sur nouveau code **> 80%**
- Duplication sur nouveau code **< 3%**

---

## Securite

### Issues corrigees

| Issue | Fichier | Correction |
|-------|---------|------------|
| Cle JWT hardcodee | `AuthController.cs` | Configuration externalisee |
| Cle JWT hardcodee | `Program.cs` | Configuration externalisee |

### Bonnes pratiques

- Ne jamais commiter de secrets dans le code
- Utiliser les variables d'environnement ou `appsettings.json`
- Configurer `SONAR_TOKEN` comme secret GitHub
