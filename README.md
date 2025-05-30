Simulador de Corrida com Algoritmo Genético

Bem-vindo ao Simulador de Corrida com Algoritmo Genético!
Este projeto é uma simulação de corrida onde carros autônomos evoluem ao longo de gerações, utilizando um algoritmo genético para melhorar seu desempenho. Abaixo, você encontrará uma explicação detalhada sobre como o jogo funciona, a lógica por trás do algoritmo genético e como os carros aprendem a percorrer todo o circuito de forma eficiente.

Visão Geral do Jogo
O objetivo do jogo é criar uma população de carros autônomos que aprendem a completar um circuito de corrida do início ao fim. Cada carro utiliza sensores e mapeamento do ambiente para se orientar e evitar obstáculos. Ao longo das gerações, os carros evoluem para percorrer toda a pista corretamente, aprendendo com seus erros e otimizando seu comportamento.

Como Funciona o Jogo
População Inicial
No início do jogo, uma população de carros é criada. Cada carro é uma instância de um prefab (modelo de carro) e possui um conjunto de genes que define seu comportamento.

Os genes são representados por um array de números reais (float[]), onde cada valor influencia a forma como o carro acelera, freia, e vira.

Percurso e Avaliação
Os carros devem percorrer o circuito completo do início ao fim, usando sensores e raycasts para detectar obstáculos e guiar seus movimentos. O ambiente é mapeado conforme os carros se movimentam, e eles aprendem a usar essas informações para tomar melhores decisões.

Durante a corrida, o desempenho de cada carro é medido por uma função de fitness, que considera:

Quantidade de waypoints alcançados.

Progresso total na pista (distância percorrida).

Tempo para completar o percurso.

Velocidade média.

Carros que batem em paredes, saem da pista ou ficam parados por muito tempo são penalizados e removidos da corrida.

Evolução
Ao final de cada geração (após um tempo pré-definido ou quando todos os carros param), os melhores carros são selecionados para gerar a próxima geração.

O processo inclui:

Seleção: Os carros com maior fitness são escolhidos como pais.

Crossover: Os genes dos pais são combinados para criar filhos.

Mutação: Pequenas alterações aleatórias são aplicadas aos genes para introduzir diversidade.

Gerações Sucessivas
Com o passar das gerações, os carros aprendem a completar a pista inteira de forma cada vez mais eficiente, ajustando seu comportamento com base na experiência acumulada. O jogo mostra em tempo real:

Número da geração atual.

Melhor fitness da geração.

Tempo de volta ou tempo total percorrido pelo melhor carro.

Detalhes Técnicos
Algoritmo Genético
O algoritmo genético simula a evolução natural, onde os melhores indivíduos (carros) passam suas características para a próxima geração.

a. Representação dos Genes
Cada carro possui um array de genes (float[]), onde cada gene controla:

Intensidade da aceleração.

Intensidade da frenagem.

Intensidade da rotação.

b. Fitness
A função de fitness avalia o desempenho com base em:

Quantidade de waypoints ou segmentos da pista alcançados.

Tempo gasto para avançar.

Distância total percorrida.

Penalidades por colisões ou inatividade.

c. Seleção
Os carros são ordenados pelo fitness, e os melhores são usados como pais. Pode-se usar elitismo para manter os melhores indivíduos intactos na próxima geração.

d. Crossover
Crossover uniforme: cada gene do filho tem 50% de chance de vir de um dos pais.

Exemplo:

Pai A: [0.1, 0.5, 0.3]

Pai B: [0.4, 0.2, 0.7]

Filho: [0.1, 0.2, 0.7]

e. Mutação
Os genes dos filhos podem sofrer mutações aleatórias, com uma taxa de mutação definida (mutationRate), permitindo o surgimento de novos comportamentos.

Controle dos Carros
Cada carro é controlado por um script (CarBrain) que utiliza os genes para tomar decisões com base nas leituras dos sensores (como raycasts). As decisões incluem:

Quando acelerar ou frear.

Quando virar para a esquerda ou direita.

O comportamento é dinâmico e se ajusta conforme o ambiente é percebido pelos sensores.

Interface do Usuário
O jogo apresenta informações úteis como:

Geração atual.

Melhor fitness da geração.

Tempo de volta ou tempo total do melhor carro.

Essas informações ajudam a acompanhar a evolução dos carros ao longo do tempo.
