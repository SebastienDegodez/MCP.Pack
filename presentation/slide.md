---
marp: true
theme: custom-default
class:
 - invert
author: DEGODEZ Sébastien
---
<!--
_class:
 - lead
 - invert
paginate: skip
-->

# Model Context Protocol (MCP)

## Use Cases pour Développeurs C# 🚀

*Standardiser la façon dont les applications fournissent du contexte aux LLMs*

![bg opacity:.7](https://raw.githubusercontent.com/SebastienDegodez/slide-marp/refs/heads/main/presentation/images/bg.jpg)

---
<!-- 
_class:
 - lead
 - invert
footer: ''
paginate: skip
-->

![bg opacity:.7](https://raw.githubusercontent.com/SebastienDegodez/slide-marp/refs/heads/main/presentation/images/bg.jpg)

<div id="presentation">  
   <img src="https://raw.githubusercontent.com/SebastienDegodez/slide-marp/refs/heads/main/presentation/images/me.png" alt="Photo" />
   <div>
      <h1>DEGODEZ Sébastien</h1>
      <h5>Software Engineer chez AXA en France</h5>
   </div>
</div>

<i class="fa-brands fa-x-twitter"></i> Twitter: seb2goxdev
<i class="fa-brands fa-linkedin"></i> LinkedIn: SebastienDegodez
<i class="fa-brands fa-github"></i> GitHub: SebastienDegodez

---
<!-- 
paginate: true
footer: 'DEGODEZ Sébastien - <i class="fa-brands fa-linkedin"></i> in/sebastien-degodez'
-->

## 🎯 Qu'est-ce que MCP ?

> **MCP est comme un port USB-C pour les applications AI**
> 
> Un protocole ouvert qui standardise la connexion entre les modèles AI et les sources de données/outils


<!-- C'est un standard permettant de développer des outils -->

---

### Le Problème Avant MCP
```
// ❌ Approche traditionnelle : APIs spécifiques
var chatGPT = new OpenAIClient();
var claude = new AnthropicClient(); 
var gemini = new GoogleClient();
// Chaque intégration unique !
```

---

### La Solution MCP
```
// ✅ Avec MCP : Interface standardisée
var mcpClient = new McpClient();
mcpClient.ConnectToServer("file-system");
mcpClient.ConnectToServer("database-server");
// Un protocole, multiple serveurs !
```

### Avantages pour l'IA Agentic 🤖
- **Autonomie** : L'IA peut découvrir et utiliser des outils
- **Flexibilité** : Changement de provider sans recodage
- **Sécurité** : Données restent dans votre infrastructure
---

## 🏗️ Architecture MCP

```
┌───────────────────────────────┐          ┌─────────────────┐
│        MCP HOST               │          │   MCP SERVER    │
│  ┌─────────────────────────┐  │          │                 │
│  │ Claude Desktop          │  │◄───────► │ NuGet Server    │
│  │ VS Code                 │  │ JSON-RPC │ Database API    │
│  │ Custom Apps             │  │ over     │ File System     │
│  │                         │  │ Transport│ Weather API     │
│  │  ┌─────────────────┐    │  │          │ Git Server      │
│  │  │   MCP CLIENT    │    │  │          └─────────────────┘
│  │  │ (embedded)      │    │  │                  │
│  │  │ • Protocol      │    │  │                  ▼
│  │  │ • Connections   │    │  │         ┌─────────────────┐
│  │  │ • 1:1 per server│    │  │         │ LOCAL/REMOTE    │
│  │  └─────────────────┘    │  │         │ DATA SOURCES    │
│  └─────────────────────────┘  │         │                 │
└───────────────────────────────┘         │ • Databases     │
                                          │ • APIs          │
                                          │ • Files         │
                                          └─────────────────┘
```

--- 
## 🏗️ Architecture MCP

- **MCP Host** : Application consommatrice avec client MCP intégré. Celle qui comprends le protocole JSON-RPC.
- **MCP Client** : Intégré dans le Host, gère les connexions 1:1
- **MCP Server** : Programme léger exposant des capacités via MCP

<!-- Notes de présentation : 
- Insister sur le fait que le client MCP est INTÉGRÉ dans l'application host
- Un host peut se connecter à plusieurs serveurs simultanément
- Chaque serveur expose ses propres tools/resources/prompts
- La communication se fait via JSON-RPC sur différents transports -->


---

## 🔧 Concepts MCP vs HTTP

### 📋 **Resources** = GET
> *Récupérer des données/contenus*

```csharp
// HTTP GET
GET /api/docs?status=available

// MCP Resource
[McpServerResource]
public static async Task<ApiDocInfo[]> ListApiDocumentations()
{
    return await _documentationService.GetAvailableDocsAsync();
}
```

<!-- Notes de présentation :
- Resources permettent de LIRE des données existantes
- Équivalent HTTP GET : récupération sans modification
- L'IA peut découvrir quelles docs sont disponibles avant d'agir
- Retourne toujours des données structurées (JSON/objets)
- Exemple concret : lister les projets, fichiers, bases de données disponibles -->

---

### 🛠️ **Tools** = POST
> *Exécuter des actions*

```csharp
// HTTP POST
POST /api/docs/generate { "projectPath": "/src/MyProject" }

// MCP Tool
[McpServerTool, Description("Generate API documentation")]
public static async Task<DocumentationResult> GenerateApiDocs(string projectPath)
{
    return await _documentationService.GenerateAsync(projectPath);
}
```

<!-- Notes de présentation :
- Tools permettent d'EXÉCUTER des actions et modifications
- Équivalent HTTP POST : création, modification, suppression
- L'IA peut effectuer des opérations concrètes sur les systèmes
- Peuvent avoir des effets de bord (écriture fichiers, BDD, API calls)
- Exemple concret : générer fichiers, envoyer emails, créer ressources -->

---

### 💬 **Prompts** = Templates
> *Workflows pré-définis*
> Le guide pour l'IA.

```csharp
// Template de requête réutilisable
[McpServerPrompt, Description("Generate API documentation")]
public static PromptMessage GenerateApiDocs(string projectPath)
{
    return new PromptMessage($"""
        Analyze the project at {projectPath} and generate API documentation.
        Focus on public endpoints and data models.
        """);
}
```

<!-- Notes de présentation :
- Prompts sont des TEMPLATES qui guident l'IA
- Fournissent le contexte et les instructions pour une tâche
- Réutilisables : même prompt pour différents projets
- Standardisent la qualité des réponses de l'IA
- Exemple concret : templates de code review, documentation, tests -->

---
<!--
_class:
 - lead
 - invert
paginate: skip
-->

# 🚀 Transports MCP
#### 📟 STDIO, 📡 SSE ou 🌐 Streamable HTTP

---

##### 📟 **Standard Input/Output (Stdio)**

- 🧩 **Usage** : Créer des outils CLI, Exécuter des scripts shell
- 🧩 **Usage** : Interagir avec des applications locales
- 🧩 **Usage** : Besoin simple de communication entre processus
&nbsp;
- ✅ **Avantages** : Sécurité implicite (pas de port réseau), Pas de dépendance réseau
- ✅ **Avantages** : Performance élevée (pas de latence réseau)
&nbsp;
- ❌ **Inconvénients** : Limité à des échanges locaux, Fragile

<!-- Notes de présentation :
- STDIO: Permet des échanges de données entre processus sans nécessiter de réseau, mais peut être fragile si une erreur se produit dans la sortie et limité à des environnements locaux.

-->
---

##### 📡 **Server-Sent Events (SSE)**

_Déprécié_ depuis la version 2024-11-05, remplacé par Streamable HTTP


<!-- Notes de présentation :
En cas de perte de flux, pas de reprise automatique
Déprécié depuis la version 2024-11-05, remplacé par Streamable HTTP
-->

---

##### 🌐 **Streamable HTTP**

- 🧩 **Usage** : Besoin de supporter plusieurs clients simultanés
- 🧩 **Usage** : Besoin d'un communication HTTP
&nbsp;
- ✅ **Avantages** : Standard web compatible HTTP/2 & HTTP/3
- ✅ **Avantages** : Scalabilité et load balancing possibles
- ✅ **Avantages** : Reprise automatique en cas de perte de flux
&nbsp;
- ❌ **Inconvénients** : Plus lourd que STDIO
- ❌ **Inconvénients** : Nécessite un serveur HTTP compatible


<!-- Notes de présentation :
- Stdio : standard de production, plus sécurisé car pas de port réseau exposé
- HTTP : futur transport qui permettra la scalabilité et le load balancing
- La beauté de MCP : même code, différents transports selon le contexte -->

---

## 🔧 Use Cases Principaux

### 1. **Installation du Package MCP** 📦
```bash
# Installation du template MCP
dotnet new install ModelContextProtocol.AspNetCore
```

### Packages NuGet inclus
- `ModelContextProtocol` : Core MCP functionality

---

## 🏗️ Configuration du Startup

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.WithMcpServer(args)
    .WithStdioServerTransport() // Pour le mode Stdio
    .WithSseServerTransport()   // Pour le mode SSE
    .WithTools<MyTool>()          // Charger des Tools spécifiques
    .WithResources<MyResource>()  // Charger des Resources spécifiques
    .WithPrompts<MyPrompt>();      // Charger des Prompts spécifiques

var app = builder.Build();

// Mapping des endpoints MCP selon le transport
app.MapMcpServer(args);
app.Run();
```

---

### 🔍 Chargement automatique

```csharp
builder.WithMcpServer(args)
    .WithToolsFromAssembly()      // Scan automatique des Tools `[McpServerTool]`
    .WithResourcesFromAssembly()  // Scan automatique des Resources `[McpServerResource]`
    .WithPromptsFromAssembly();   // Scan automatique des Prompts `[McpServerPrompt]`

var app = builder.Build();
```

<!-- Notes de présentation :
Il est possible d'utiliser FromAssembly pour charger automatiquement les Tools, Resources et Prompts

### Ce qui est configuré automatiquement
- **Tools** : Méthodes marquées `[McpServerTool]`
- **Resources** : Méthodes marquées `[McpServerResource]`
- **Prompts** : Méthodes marquées `[McpServerPrompt]`
-->

---

## 🛠️ Exemple de Tool (Action)

### Créer un nouveau contrat d'assurance

```csharp
[McpServerToolType]
public static class AssuranceTool
{
    [McpServerTool, Description("Create new insurance contract")]
    public static async Task<ContratResult> CreateContract(
        string customerId, string productType, decimal coverageAmount, DateTime startDate)
    {
        // Validation des données
        // Code simplifié pour l'exemple
        ContratResult result = await _contractUseCase.CreateAsync(contract);

        return result;
    }
}
```

---

## 📋 Exemple de Resource (Lecture)

### Lister les contrats d'assurance

```csharp
[McpServerResourceType]
public static class AssuranceResource
{
    [McpServerResource, Description("List insurance contracts")]
    public static async Task<Contrat[]> ListContractsInsurance(
        string customerId, string status, DateTime startDate)
    {
        // Logique contrat : appel au use case qui va chercher les contrats
        return await _getContractsUseCase.ExecuteAsync(customerId, status, startDate);
    }
}
```

---

## 💬 Exemple de Prompt (Template)

### Template d'Analyse de Risque

```csharp
[McpServerPromptType]
public static class AssurancePrompt
{
    [McpServerPrompt, Description("Analyze insurance risk profile")]
    public static PromptMessage AnalyzeRiskProfile(
        string customerId, string productType)
    {
        return new PromptMessage($"""
            Please analyze the insurance risk profile for:
            - Customer ID: {customerId}
            - Product Type: {productType}
            
            Focus on:
            1. **Historical Claims**: Review past claims frequency and severity
            2. **Customer Profile**: Age, location, occupation, lifestyle factors
            3. **Product Specifics**: Coverage type, limits, deductibles
            4. **Market Trends**: Current market conditions and regulatory changes
            5. **Risk Factors**: Identify high-risk elements and mitigation strategies
            
            For each risk factor, provide:
            - Risk level (Low/Medium/High)
            - Impact assessment
            - Recommended premium adjustment
            - Preventive measures suggestions
            - Underwriting recommendations
            
            Format your response as a comprehensive risk assessment report 
            with actionable insights for underwriters and pricing teams.
            
            Include specific recommendations for:
            - Premium pricing strategy
            - Policy terms and conditions
            - Risk mitigation requirements
            - Monitoring and review schedule
            """);
    }
}
```

---

## 🚀 Avantages pour les Développeurs

<div class="columns">
<div>

#### ✅ **Flexibilité**
- Changement de fournisseur LLM sans modification de code
- Intégrations pré-construites disponibles


#### ✅ **Légèreté**
- Support AOT compilation

</div>
<div>

#### ✅ **Extensibilité**
- Protocole standardisé
- Écosystème en croissance

#### ✅ **Sécurité**
- Implémentation en cours par le projet MCP

</div>
</div>

---

## 🚀 Avantages pour les Développeurs

#### ✅ **Clean Architecture / Hexagonale**
- **Adapters parfaits** : MCP servers = ports d'entrée pour l'IA
- **Inversion de dépendance** : Use cases indépendants du transport MCP
- **Séparation des responsabilités** : Logique métier isolée des détails MCP
- **Testabilité** : Mock des MCP servers facilement

---

### Test avec MCP Inspector

```bash
npx @modelcontextprotocol/inspector
```

<!-- Notes de présentation :
- Le template hybride permet de basculer entre SSE et Stdio facilement
- MCP Inspector est l'outil de debugging officiel
- Démo live possible : montrer le template en action
- Encourager les développeurs à tester avec leurs propres use cases -->

---

## 📚 Ressources

- **Documentation** : [modelcontextprotocol.io](https://modelcontextprotocol.io)

### 🤝 Questions ?


