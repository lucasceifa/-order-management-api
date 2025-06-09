# Order Management System

## 📦 Projeto Full Stack para Gerenciamento de Pedidos

Este é um sistema completo de gerenciamento de pedidos, com **frontend em HTML + jQuery + Bootstrap** e **backend em ASP.NET Core**, utilizando **SQL Server** para persistência de dados.

---

## ✅ Requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- SQL Server (Express, Developer ou LocalDB)
- Visual Studio 2022 ou superior
- Extensão **Live Server** no VSCode

---

## 🚀 Como rodar o projeto

### 1. Executar o Frontend

1. Abra o Visual Studio Code na pasta:  
   `FRONTEND-ORDER-SYSTEM`
2. Instale e ative a extensão **Live Server**.
3. Clique com o botão direito no `index.html` e selecione:  
   **"Open with Live Server"**.
4. O frontend estará acessível via `http://127.0.0.1:5500`.

> ⚠️ O backend precisa estar rodando para que as requisições funcionem corretamente.

---

### 2. Criar o Banco de Dados SQL

1. Crie um banco de dados SQL local em sua máquina (ex: `OrderDb`).
2. Deixe seu servidor SQL ativo (`localhost`, `localhost\SQLEXPRESS`, etc.).

---

### 3. Configurar e Executar o Backend

1. Abra o projeto no Visual Studio:  
   `OrderManagement.sln`
2. Vá até o arquivo:

   ```
   backend-order-system/OrderManagement/OrderManagement/appsettings.Development.json
   ```

3. Atualize a `DefaultConnection` com a string do seu banco. Exemplo:

   ```json
   "DefaultConnection": "Server=localhost\SQLEXPRESS;Database=OrderDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   ```

4. Rode o projeto com o perfil da API selecionado (`OrderManagement.API`).

---

### 4. Validação

- A primeira execução criará automaticamente:
  - ✅ 20 clientes
  - ✅ ~40 produtos
- Verifique se a aplicação está rodando na porta `44319`.  
  Você pode confirmar isso em `launchSettings.json`.
- Se tudo estiver correto, o frontend estará funcional com o backend.

---

## 🧩 Funcionalidades

- CRUD completo de **Clientes**, **Produtos** e **Pedidos**
- Validações no frontend e backend
- Toasts de erro e sucesso com Bootstrap
- Interfaces responsivas
