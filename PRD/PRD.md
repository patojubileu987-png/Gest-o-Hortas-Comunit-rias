# PRD - Sistema de Gestão de Hortas Comunitárias

## 1. Visão Geral
Desenvolver um sistema para gestão de hortas comunitárias que organize canteiros, participantes, cultivos, tarefas e colheitas. A solução deve permitir o controle de uso de canteiros, acompanhamento do ciclo de plantio e colheita, além de garantir regras de compatibilidade entre cultivos. Deve ser construída respeitando a estrutura de camadas definida no PRD.

## 2. Objetivos
- Permitir cadastro e manutenção de canteiros com dados de localização, área, tipo de solo e status.
- Gerenciar participantes responsáveis por cultivos e tarefas.
- Registrar cultivos vinculados a canteiros com espécie, data de plantio, previsão de colheita e responsáveis.
- Controlar tarefas de manutenção e acompanhar colheitas.
- Impedir o uso simultâneo de um canteiro por cultivos incompatíveis.
- Exibir cultivos ativos e histórico de colheitas.

## 3. Escopo
### Inclui
- Cadastro, edição e exclusão de canteiros.
- Cadastro, edição e exclusão de participantes.
- Registro de cultivos e associação a canteiros.
- Registro de tarefas e colheitas.
- Consulta de cultivos ativos.
- Regras de compatibilidade entre cultivos em um mesmo canteiro.
- Interface para visualização dos principais dados.

### Fora do escopo inicial
- Relatórios complexos.
- Integração com sensores ou IoT.
- Módulo financeiro.

## 4. Requisitos Funcionais

### 4.1 Gestão de Canteiros
- RF1: Cadastrar canteiro com localização, área, tipo de solo e status (ativo, em manutenção, inativo, etc.).
- RF2: Editar e excluir canteiros.
- RF3: Listar canteiros e filtrar por status e tipo de solo.

### 4.2 Gestão de Participantes
- RF4: Cadastrar participantes com nome, contato e papel.
- RF5: Atribuir participantes como responsáveis por cultivos e tarefas.
- RF6: Consultar participantes e seus cultivos ou tarefas associadas.

### 4.3 Gestão de Cultivos
- RF7: Registrar cultivo associado a um canteiro.
- RF8: Cada cultivo deve ter: espécie plantada, data de plantio, previsão de colheita e responsáveis.
- RF9: Editar cultivo enquanto estiver ativo.
- RF10: Consultar cultivos ativos e históricos.

### 4.4 Gestão de Tarefas
- RF11: Registrar tarefas de manutenção por cultivo ou canteiro.
- RF12: Atribuir responsáveis e status a cada tarefa.
- RF13: Atualizar andamento e conclusão de tarefas.

### 4.5 Gestão de Colheitas
- RF14: Registrar colheitas com data, quantidade e cultivo associado.
- RF15: Acompanhar histórico de colheitas por cultivo e canteiro.

### 4.6 Regras de Negócio
- RF16: Impedir uso simultâneo do mesmo canteiro por cultivos incompatíveis.
- RF17: Validar, ao criar novo cultivo em canteiro ocupado, se já existe cultivo ativo incompatível.
- RF18: Permitir somente cultivos compatíveis coexistirem no mesmo canteiro.
- RF19: Mostrar avisos sobre canteiros com conflitos.

## 5. Requisitos Não Funcionais
- RNF1: Separação clara de camadas (apresentação, aplicação, domínio, infraestrutura).
- RNF2: Interface simples e intuitiva.
- RNF3: Persistência em banco de dados relacional.
- RNF4: Validação de dados no backend.
- RNF5: Performance adequada para até centenas de canteiros e cultivos.
- RNF6: Segurança básica de controle de acesso, se aplicável.

## 6. Modelo de Dados Proposto
- Canteiro: id, localização, área, tipoSolo, status.
- Participante: id, nome, contato, papel.
- Cultivo: id, canteiroId, especie, dataPlantio, previsaoColheita, status.
- CultivoResponsavel: cultivoId, participanteId.
- Tarefa: id, cultivoId / canteiroId, descricao, responsavelId, status, dataPrevista, dataConclusao.
- Colheita: id, cultivoId, data, quantidade, observacoes.
- CompatibilidadeCultivo: especieA, especieB, compatibilidade.

## 7. Fluxo de Usuário
1. Acessar dashboard de hortas.
2. Cadastrar ou selecionar um canteiro.
3. Visualizar cultivos ativos e status do canteiro.
4. Criar novo cultivo vinculando canteiro e responsáveis.
5. Agendar tarefas de plantio, rega ou manutenção.
6. Registrar colheita quando ocorrer.
7. Consultar cultivos ativos e histórico de colheitas.

## 8. Camadas da Solução
- Apresentação: páginas e componentes para cadastro e consulta.
- Aplicação: serviços que coordenam os casos de uso.
- Domínio: regras de negócio e validações de cultivo, compatibilidade e status.
- Infraestrutura: repositórios, acesso a banco de dados e persistência.

## 9. Critérios de Aceitação
- O sistema cadastra e edita canteiros, participantes e cultivos.
- O sistema registra tarefas e colheitas.
- O sistema exibe cultivos ativos.
- O sistema bloqueia cultivos incompatíveis no mesmo canteiro.
- A solução segue a estrutura de camadas no desenvolvimento.
