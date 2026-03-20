# Replicador

Aplicação desenvolvida em .NET para replicação de dados entre bancos de dados e sistemas, responsável por sincronizar informações entre ambientes diferentes, garantindo que registros sejam enviados, atualizados ou inseridos corretamente.

O Replicador é utilizado em cenários onde existe comunicação entre matriz, filial ou sistemas distintos, sendo executado automaticamente para manter os dados sincronizados.

---

## 📌 Objetivo

O sistema tem como finalidade:

* Replicar dados entre bancos
* Sincronizar registros
* Executar rotinas automáticas
* Garantir consistência de dados
* Integrar sistemas diferentes
* Evitar divergência entre bases

Pode ser usado para:

* Matriz / Filial
* ERP / API
* Banco local / banco remoto
* Sistemas distribuídos

---

## 📌 Funcionamento geral

Fluxo do sistema:

```id="9q2v4n"
Inicia replicador
↓
Conecta banco origem
↓
Conecta banco destino
↓
Busca registros
↓
Verifica diferenças
↓
Insere / atualiza
↓
Registra log
↓
Finaliza ciclo
```

O processo pode ser executado manualmente ou automático.

---

## 📌 Estrutura do projeto

Arquivos comuns:

```id="c8k1m7"
Program.cs
Replicador.cs
Form1.cs
Form1.Designer.cs
App.config
Models/
Dao/
Utils/
Replicador.csproj
Properties/
```

Descrição:

| Arquivo / Pasta | Função               |
| --------------- | -------------------- |
| Program.cs      | Inicialização        |
| Form1.cs        | Tela principal       |
| Replicador.cs   | Lógica de replicação |
| Dao             | Acesso ao banco      |
| Models          | Entidades            |
| Utils           | Funções auxiliares   |
| App.config      | Configuração         |
| csproj          | Projeto              |

---

## 📌 Program.cs

Responsável por iniciar o sistema.

```csharp id="5d7f2x"
Application.Run(new Form1());
```

Função:

* iniciar aplicação
* abrir tela
* iniciar processo

---

## 📌 Form1.cs

Tela principal.

Responsável por:

* iniciar replicação
* mostrar status
* registrar logs
* controlar execução

Pode conter:

* timer
* botão iniciar
* botão parar

---

## 📌 Replicador.cs

Classe responsável pela replicação.

Funções:

* ler dados
* comparar registros
* inserir
* atualizar
* sincronizar

Fluxo:

```id="t3r8k6"
Ler origem
↓
Comparar destino
↓
Inserir / atualizar
↓
Salvar
```

---

## 📌 App.config

Arquivo de configuração.

Pode conter:

```id="4x6p9q"
ConnectionOrigem
ConnectionDestino
Intervalo
Tabela
Servidor
```

Usado para definir bancos.

---

## 📌 Models

Representam dados replicados.

Exemplo:

```id="1c9f2m"
Cliente
Produto
NotaFiscal
Pedido
Estoque
```

Usado para transportar dados.

---

## 📌 Dao / Repository

Responsável por acesso ao banco.

Funções:

* executar SQL
* consultar dados
* inserir
* atualizar

Pode acessar:

* Firebird
* SQL Server
* PostgreSQL
* MySQL

---

## 📌 Utils

Funções auxiliares.

Pode conter:

* logs
* conversões
* validações
* helpers

---

## 📌 Processo de replicação

Etapas:

1. Conectar banco origem
2. Conectar banco destino
3. Buscar registros novos
4. Comparar
5. Inserir / atualizar
6. Registrar log

Fluxo:

```id="7n3b5v"
Origem → Replicador → Destino
```

---

## 📌 Logs

O sistema pode registrar:

* início
* fim
* erro
* registro replicado

Pode salvar em:

* tela
* arquivo
* banco

---

## 📌 Requisitos

* .NET instalado
* Banco configurado
* Permissão de rede
* Permissão de acesso ao banco

---

## 📌 Como executar

1. Abrir solução

```id="x2c6p1"
Replicador.sln
```

2. Compilar

3. Executar

ou

```id="f8r4n9"
Replicador.exe
```

---

## 📌 Uso recomendado

* Sincronização de dados
* Integração matriz / filial
* Replicação entre bancos
* Integração ERP
* Automação de dados

---

## 📌 Autor

Gabriel Rebouças
LSI Sistemas

---
