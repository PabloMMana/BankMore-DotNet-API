Este projeto é uma API de sistema bancário robusta, desenvolvida como parte de um desafio técnico para Desenvolvedor .NET. A solução foca em integridade de dados, performance e uma arquitetura limpa e escalável.

🛠 Tecnologias e Padrões
.NET 8: Framework principal.

Dapper: Micro-ORM utilizado para garantir máxima performance nas consultas SQL.

SQL Server: Banco de dados relacional.

MediatR: Implementação do padrão CQRS (Command Query Responsibility Segregation) para desacoplar a lógica de negócio.

Swagger (OpenAPI): Documentação interativa e testável dos endpoints.

Segurança: Armazenamento de senhas utilizando Hash e Salt.

Arquitetura: Organizada em camadas (Domain, Application, Infrastructure e API).

📌 Funcionalidades Principais
Gestão de Contas: Cadastro de novos usuários e ativação/desativação de contas.

Autenticação: Sistema de login seguro.

Transações Bancárias:

Depósito: Entrada de valores via número da conta.

Transferência: Movimentação entre contas com validação de saldo e atomicidade (Transaction).

Extrato: Histórico detalhado de todas as operações financeiras da conta.

⚙️ Como Executar o Projeto
1. Requisitos
Visual Studio 2022.

SDK do .NET 8 instalado.

SQL Server (Express).

2. Configuração do Banco de Dados
Execute o script abaixo no seu SQL Server para criar a estrutura necessária:

SQL
-- Script simplificado (adicione aqui o script que usamos para criar contacorrente e transacao)
CREATE TABLE contacorrente (...);
CREATE TABLE transacao (...);
3. Ajuste de Configuração
No arquivo appsettings.json do projeto BankMore.Api, ajuste a ConnectionString para apontar para o seu servidor local:

JSON
"ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=BankMoreDB;Trusted_Connection=True;"
}
4. Execução
Pressione F5 no Visual Studio ou execute o comando:

Bash
dotnet run --project BankMore.Api
Acesse a documentação em: https://localhost:7051/swagger

👨‍💻 Autor
Pablo Mana [Seu LinkedIn - Opcional]
