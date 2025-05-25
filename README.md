# Agenda de Compromissos
**Equipe:** Emilly Pessutti

Projeto desenvolvido para a Atividade Prática #04 da disciplina de Programação Orientada a Objetos (POO), com o objetivo de aplicar conceitos de abstração, encapsulamento, associações, composição e persistência de dados em C#.

---

## Objetivo do Projeto

Criar um sistema de compromissos que funcione via linha de comando (CLI), utilizando serialização JSON para salvar e recuperar dados, com foco total em Programação Orientada a Objetos.

---

## Como executar

1. No terminal, navegue até a pasta do projeto:
```bash
cd AgendaDeCompromisso
```
2. Execute o comando e abaixo e compile o projeto:
```bash
dotnet run
```

---

## Como testar

1. Liste os compromissos com listar
```bash
dotnet run -- listar
```

2. Adicione compromissos com o comando adicionar
```bash
dotnet run -- adicionar <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>
```

3. Edite qualquer compromisso usando o índice
```bash
dotnet run -- editar <indice> <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>
```

4. Exclua um compromisso se desejar
```bash
dotnet run -- excluir <indice>
```

---

## Observações

- Ao adicionar ou editar compromissos pela linha de comando, todos os campos devem ser informados na ordem correta.
- Se você quiser manter o valor atual de um campo ao editar, use aspas duplas vazias ("") para aquele campo.
- Os campos **participantes** e **anotações** são **opcionais**:
  - Você pode deixar em branco usando `""`.
  - Se desejar adicionar mais de um participante ou anotação, separe com vírgulas.

Exemplos de uso:

**Adicionar um compromisso (com campo de anotações em branco):**
```bash
dotnet run -- adicionar Emilly 12/09/2025 12:00 "Aula de POO" "Sala L18" 3 "Daniel,Mariana" ""
```

**Editar apenas a descrição de um compromisso:**
```bash
dotnet run -- editar 1 "" "" "" "Nova descrição" "" "" "" ""
```

**Editar apenas a capacidade:**
```bash
dotnet run -- editar 1 "" "" "" "" "" 5 "" ""
```

---

# SOBRE OS DADOS

- Todas as informações dos compromissos são salvas automaticamente no arquivo compromissos.json após cada operação (inclusão, edição ou exclusão).

- Os dados são automaticamente salvos em compromissos.json após cada operação
