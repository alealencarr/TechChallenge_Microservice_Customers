# language: pt-BR
Funcionalidade: Criar Cliente
    Como um sistema de autoatendimento
    Eu quero identificar ou cadastrar clientes pelo CPF
    Para personalizar o pedido

Cenario: Cadastrar um novo cliente com sucesso
    Dado que eu tenho um cliente novo com CPF "73656066060" nome "Joao" e email "joao@email.com"
    E que este CPF nao existe na base de dados
    Quando eu solicito o cadastro deste cliente
    Entao o cliente deve ser salvo no repositorio
    E o sistema deve informar que foi um novo cadastro

Cenario: Identificar cliente ja existente
    Dado que eu informo um CPF "73656066060" que ja esta cadastrado
    Quando eu solicito o cadastro deste cliente
    Entao o sistema deve retornar o cliente existente
    E o sistema deve informar que o cliente ja existia
    E nao deve tentar salvar duplicado

Cenario: Tentar cadastrar com dados invalidos
    Dado que eu tento cadastrar um cliente com email invalido
    Quando eu solicito o cadastro deste cliente
    Entao o sistema deve retornar um erro de validacao