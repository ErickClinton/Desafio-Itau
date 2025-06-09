# 💼 Desafio de Investimentos Itaú

Sistema de controle de investimentos com funcionalidades para:

- Visualizar total investido por ativo
- Acompanhar posição por papel
- Calcular lucro/prejuízo global
- Ver total de corretagem por cliente
- Consultar cotações em tempo real
- Comprar e vender ativos

---

## 🚀 Como rodar o projeto localmente

### 📦 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js](https://nodejs.org/)
- [Docker](https://www.docker.com/) (opcional)
- Banco: MySQL 8 ou superior

---

### 🔧 Configuração do Back-End



1. Crie um arquivo `.env` com base no `.env.example`.

3. Execute as migrações:

```bash
dotnet ef database update
```
4. Rode a aplicação:
```bash
dotnet run
```

## 🐳 Subindo tudo com Docker

Caso deseje subir o **back-end** + **banco de dados** com Docker:

1 - Esteja na pasta onde está o docker-compose
2. Execute o comando abaixo:

```bash
docker-compose up --build
```

Observação importante:
Caso opte por não utilizar Docker, será necessário subir o Kafka e o MySQL manualmente.
A aplicação já está configurada para criar automaticamente os tópicos necessários no Kafka durante a inicialização.

## 📚 Endpoints disponíveis

| Método | Rota                                           | Descrição                         |
|--------|------------------------------------------------|-----------------------------------|
| GET    | `/investments/user/{id}/total-invested`        | Retorna o total investido por ativo de um usuário |
| GET    | `/investments/user/{id}/position`              | Retorna a posição por papel de um usuário         |
| GET    | `/investments/user/{id}/total-brokerage`       | Retorna o total de corretagem do usuário          |
| GET    | `/investments/user/{id}/global-position`       | Retorna o lucro ou prejuízo global do usuário     |
| GET    | `/quotation/{assetCode}`                       | Retorna a última cotação de um ativo              |
| POST   | `/trades/buy`                                  | Realiza a compra de um ativo                      |
| POST   | `/trades/sell`                                 | Realiza a venda de um ativo                       |

