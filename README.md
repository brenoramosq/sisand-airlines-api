# Sisand Airlines - Sistema de Compra de Passagens Aéreas

## 1) Descrição
Este projeto é uma **API de compra de passagens aéreas** desenvolvida para a empresa fictícia **Sisand Airlines**, que realiza **viagens diárias de Curitiba para São Paulo**.

A aplicação foi construída com as seguintes tecnologias:

- **Linguagem:** C#  
- **Framework:** .NET 8  
- **Tipo:** API RESTful  
- **Banco de dados:** PostgreSQL  
- **ORM/Mapper:** Dapper  

## 2) Instruções de Instalação

### 2.1. Clone ou baixe este repositório em sua máquina:
   
bash
   git clone https://github.com/brenoramosq/sisand-airlines-api.git

### 2.2. Abra o projeto no Visual Studio 2022

### 2.3. Execute os scripts SQL para criação do banco de dados no PostgreSQL

### 2.4. Configure a connection string no arquivo appsettings.json:
    "ConnectionStrings": {
        "Postgres": "Host=localhost;Port=5432;Database=sisand_airlines;Username=seu_usuario;Password=sua_senha"
    }

### 2.5. Execute o projeto (F5 ou via terminal)


## 3) Pré-requisitos
** .NET 8 SDK **
** Visual Studio 2022**

## 4) Instruções de Uso

## 4.1. Acesse o Swagger (geralmente em https://localhost:{porta}/swagger)

## 4.2. Crie os voos diários com o endpoint:
POST /api/v1/flight/create-daily-flights

## 4.3. Consulte os voos disponíveis
GET /api/v1/flight/available

## 4.4. Visualize os voos, escolha assentos disponíveis e reserve-os:
POST /api/v1/shopping-cart/create-with-items
   
⚠️ Atenção: até este ponto, não é necessário estar logado.

## 4.5. Para finalizar a compra (checkout), é necessário estar autenticado:
    
    4.5.1. Registre-se:
    POST /api/v1/customer/register

    4.5.2. Faça o login:
    POST /api/v1/customer/login

## 4.6. Após o login, realize o checkout e gere as passagens com os dados de embarque:
POST /api/v1/shopping-cart/checkout