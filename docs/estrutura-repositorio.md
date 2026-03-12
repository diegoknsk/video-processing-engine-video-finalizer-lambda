# Estrutura do repositório

Este documento descreve a estrutura de diretórios adotada após a reorganização (Storie-01-02), alinhada à Clean Architecture e às convenções do projeto.

## Árvore de pastas

```
<raiz>/
├── src/
│   ├── Core/                    # Reservado para Domain e Application (Clean Architecture)
│   ├── Infra/                   # Reservado para projetos de infraestrutura
│   └── InterfacesExternas/      # Pontos de entrada (API, Lambda, handlers)
│       └── VideoProcessing.Finalizer.Lambda/
├── tests/                       # Projetos de teste
│   └── VideoProcessing.Finalizer.Tests/
├── docs/
├── .github/workflows/
├── VideoProcessing.Finalizer.slnx
└── README.md
```

## Propósito das pastas

| Pasta | Propósito |
|-------|-----------|
| `src/Core/` | Projetos Domain e Application (regras de negócio, use cases). Nesta story permanece vazia. |
| `src/Infra/` | Projetos de infraestrutura (repositórios, clientes externos, implementações de ports). Nesta story permanece vazia. |
| `src/InterfacesExternas/` | Pontos de entrada: Lambda, API, handlers. Contém o projeto `VideoProcessing.Finalizer.Lambda`. |
| `tests/` | Projetos de teste (unitários, integração). |

O diretório virtual da solution (`.slnx`) segue a mesma organização: pastas virtuais `/src/Core`, `/src/Infra`, `/src/InterfacesExternas` e `/tests`.

## Convenções

- **Diretório físico = diretório virtual:** a localização dos projetos na solution reflete o caminho no disco.
- **Clean Architecture:** referência em `.cursor/rules/core-clean-architecture.mdc` e `.cursor/documents/quick-reference.md`.
- **Novos projetos:** adicionar em `src/Core`, `src/Infra` ou `src/InterfacesExternas` conforme a camada; testes em `tests/`.

## Limites da Storie-01-02

- **In scope:** reorganização de pastas, movimentação do projeto Lambda para `src/InterfacesExternas/`, movimentação dos testes para `tests/`, criação das pastas `src/Core/` e `src/Infra/` (vazias), atualização da solution e do CI/docs.
- **Out of scope:** criação de novos projetos (Domain, Application, Infra); refatoração do código interno da Lambda; alteração de comportamento funcional ou contratos de API.

Projetos Domain, Application e Infra serão criados em stories futuras.
