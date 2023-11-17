const axios = require("axios");

// JSON de benefícios
const beneficios = [
  {
    id: 2,
    categoria: 0,
    nome: "Ajuda De Custo Alimentação",
    descricao:
      "Esse é um benefício exclusivo para colaboradores e estagiários residentes de República em Belo Jardim. A República é uma residência custeada pela empresa para integrar e acomodar nossos colaboradores vindos de outras cidades, conforme Política de Benefícios.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/ajudadecusto.webp",
  },
  {
    id: 3,
    categoria: 3,
    nome: "Auxílio Leite",
    descricao:
      "Esse é um benefício concedido conforme convenção de cada negócio para as funcionárias lactantes, trata-se de um valor creditado em um cartão, equivalente a 10 latas de leite próprio para recém-nascido por um período de 06 meses. Esse benefício também se estende às mães adotantes de crianças de até 06 meses.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/auxilioleite.webp",
  },
  {
    id: 5,
    categoria: 1,
    nome: "Transporte Coletivo(Ônibus)",
    descricao:
      "Esse é um benefício concedido para todos os colaboradores de Belo Jardim. Fique atento a rota e horários. Não haverá desconto para esse benefício.https://mouramais.grupomoura.com/wiki/30/detail - Arquivos GPM - Rota de Ônibus",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/transporte.webp",
  },
  {
    id: 6,
    categoria: 3,
    nome: "Allya",
    descricao:
      "O Allya é uma plataforma de descontos gratuita que conecta os colaboradores a diversos estabelecimentos relevantes, com benefícios e vantagens financeiras. Não fique de fora desse benefício, afinal, quem não gosta de desconto?Acesse o link para conhecer. https://allya.com.br/entrar",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/allya.webp",
  },
  {
    id: 7,
    categoria: 1,
    nome: "Restaurante",
    descricao:
      "Esse é um benefício para todos os colaboradores das Unidades de Belo Jardim, Recife e Itapetininga que possuem Restaurantes.  É um serviço que oferece um cardápio variado, com opções que possam atender diferentes gostos e necessidades alimentares.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/restaurante.webp",
  },
  {
    id: 8,
    categoria: 3,
    nome: "Cesta Básica",
    descricao:
      "Prêmio mensal disponível para funcionários de Belo Jardim e Itapetininga conforme resultados de sua respectiva Unidade. A Distribuição da cesta ocorre de forma física, conforme tipo/composição de alimentos e poderá ser até duas cestas no mês. *Os colaboradores em período de experiência (03 meses) receberão uma cesta referente a produtividade.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/cestabasica.webp",
  },
  {
    id: 9,
    categoria: 3,
    nome: "Ajuda De Custo Deslocamento",
    descricao:
      "Esse benefício é para os nossos estagiários e Pessoas com Deficiência (PCD), conforme quilometragem descrita na nossa Política de Benefícios, ajudando assim o deslocamento até o trabalho com mais conforto e tranquilidade. ",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/auxiliodecustodedeslocamento.jpg",
  },
  {
    id: 10,
    categoria: 2,
    nome: "Seguro De Vida",
    descricao:
      "Esse benefício é concedido para todos os colaboradores do Grupo Moura e tem por objetivo garantir proteção financeira para os familiares e/ou pessoas que dependem do colaborador em caso de seu falecimento. Esse é um assunto muito importante e os colaboradores precisam estar atentos a atualização de sua apólice sempre que houver algum evento importante em sua vida: casamento, nascimento de filhos...",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/segurodevida.jpg",
  },
  {
    id: 11,
    categoria: 3,
    nome: "Assistência Farmacêutica",
    descricao:
      "Cartão convênio opcional para funcionários, após o período de experiência com desconto em contracheque de acordo com o parcelamento feito junto a farmácia. É necessário solicitar o cartão no Serviço Social de sua unidade. Para colaboradores em situação de afastamento previdenciário o mesmo será orientado no momento do seu afastamento.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/farmacia.webp",
  },
  {
    id: 12,
    categoria: 2,
    nome: "Plano De Saúde",
    descricao:
      "Benefício disponibilizado para todos os funcionários e dependentes legais conforme a Política de Benefícios. Cada localidade tem o seu Plano com custeio de 50% do valor para empresa e 50% para funcionário",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/planodesaude.jpg",
  },
  {
    id: 13,
    categoria: 2,
    nome: "Assistência Médica Moura - AMM",
    descricao:
      "Assistência para as Unidades de Belo Jardim com cobertura conforme rede credenciada e custeio de 50% para empresa e 50% para o colaborador e dependente conforme Política de Benefícios.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/assistenciamedicamoura.jpg",
  },
  {
    id: 14,
    categoria: 2,
    nome: "Plano Odontológico",
    descricao:
      "Benefício disponibilizado para todos os funcionários e dependentes legais conforme a Política de Benefícios. Cada localidade tem o seu plano com custeio de 50% do valor para empresa e 50% para funcionário. Nas fábricas de Belo Jardim temos os serviços do Sesi odontológico.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/assistenciaodontologica.jpg",
  },
  {
    id: 15,
    categoria: 4,
    nome: "BateMoura",
    descricao:
      "Espaço para lazer e atividades esportivas, gratuito, disponibilizado para todos os colaboradores e seus dependentes legais em Belo Jardim. O acesso deverá ser via carteirinha, gerada no Moura+ | Meu Batemoura. Fique atento as regras de utilização divulgadas em nossos canais de comunicação. Venha conhecer e se divertir!",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/batemoura.webp",
  },
  {
    id: 16,
    categoria: 3,
    nome: "Material Escolar",
    descricao:
      "Acreditamos que a educação é um grande agente de transformação, por isso, incentivamos os estudos dos filhos dos nossos colaboradores. Esse benefício é um auxílio de parcelamento para compra de material escolar, conforme convenção e Política de Benefícios.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/materialescolar.jpg",
  },
  {
    id: 17,
    categoria: 3,
    nome: "Kit Escolar",
    descricao:
      "Acreditamos que a educação é um grande agente de transformação, por isso, incentivamos os estudos dos filhos dos nossos colaboradores. Esse benefício é um BÔNUS para ajudar na compra de material escolar. Receberão esse benefício os filhos dos colaboradores, estagiários e aprendizes conforme convenção e Política de Benefícios. ",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/kitescolar.jpg",
  },
  {
    id: 18,
    categoria: 1,
    nome: "Fardamento",
    descricao:
      "Benefício disponibilizado para todos os colaboradores das Unidades de Belo Jardim e Itapetininga, deverá ser utilizado diariamente e sua despesa é 100% da empresa.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/fardamento.jpg",
  },
  {
    id: 19,
    categoria: 1,
    nome: "Estacionamento",
    descricao:
      "Local disponibilizado, sem custo, para estacionamento de veículos (carro e moto) de todos os colaboradores em suas respectivas Unidades. Atenção: A responsabilidade do veículo é inteiramente do colaborador.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/estacionamento.jpg",
  },
  {
    id: 20,
    categoria: 3,
    nome: "Prêmio De Produtividade",
    descricao:
      "Bônus variável somado ao salário do colaborador em função dos resultados de produtividade. Poderá ser concedido mensalmente para os colaboradores elegíveis conforme critérios de cada unidade fabril.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/produtividade.webp",
  },
  {
    id: 21,
    categoria: 1,
    nome: "Homenagem Por Tempo De Serviço",
    descricao:
      "É um momento de celebração que considera o tempo de serviço do colaborador a cada cinco anos. Celebramos os vínculos e reconhecemos todos que com dedicação e compromisso fazem a diferença em nossa organização.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/homenagemportempodeservico.jpg",
  },
  {
    id: 22,
    categoria: 1,
    nome: "Jeito Moura De Comemorar Com Você",
    descricao:
      "Comemoração dos eventos festivos, ao longo do ano: Dia da mulher, páscoa, Dia do Trabalho, Dia das Mães, Dia dos Pais, Dia das Crianças, Cesta Natalina e Confraternização de Final de Ano, conforme Política de Benefícios.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/eventosfestivos.jpg",
  },
  {
    id: 23,
    categoria: 1,
    nome: "República",
    descricao:
      "Esse benefício disponibiliza uma residência custeada pela empresa, que poderá ser concedido para os colaboradores das Unidades localizadas em Belo Jardim, conforme processo seletivo e Política de Benefícios.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/republica.jpg",
  },
  {
    id: 24,
    categoria: 3,
    nome: "AjudaDeCustoMoradia",
    descricao:
      "Esse benefício poderá ser concedido para funcionários contratados ou transferidos para unidades do Grupo Moura que estejam distantes de sua residência atual, conforme Política de Benefícios.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/ajudadecustomoradia.jpg",
  },
  {
    id: 25,
    categoria: 2,
    nome: "Adiantamento - Benefício Previdenciário",
    descricao:
      "É um benefício opcional, conforme convenção. Trata-se de um adiantamento do valor do salário do colaborador que se encontra em benefício previdenciário.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/adiantamento.webp",
  },
  {
    id: 26,
    categoria: 3,
    nome: "Empréstimo Consignado",
    descricao:
      "Benefício concedido para funcionários a partir de um ano da sua admissão, considerando a avaliação de sua renda. Será descontado em contracheque as parcelas acordadas com o banco.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/emprestimoconsignado.jpg",
  },
  {
    id: 27,
    categoria: 1,
    nome: "Presente De Aniversário",
    descricao:
      "Nosso jeito Moura de comemorar com nossos colaboradores. O presente de aniversário é um brinde, entregue pela liderança no dia do aniversário do colaborador(a), aprendiz e estagiário, para celebrar um dia tão especial junto daqueles que todos os dias têm estado conosco.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/presentedeaniversario.jpg",
  },
  {
    id: 28,
    categoria: 1,
    nome: "Presente De Casamento",
    descricao:
      "Nosso jeito Moura de comemorar com nossos colaboradores. O presente de casamento é um brinde entregue ao colaborador(a), estagiário ou aprendiz, após o seu casamento. Para receber, é necessário apresentar a certidão de casamento ou certidão de união estável, ao GPM/Serviço Social de sua unidade. Que essa união seja cheia de muito amor e energia para movê-los até seus objetivos. ",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/presentedecasamento.jpg",
  },
  {
    id: 29,
    categoria: 1,
    nome: "Kit Bebê",
    descricao:
      "Nosso jeito Moura de comemorar com nossos colaboradores. O kit bebê é um brinde entregue ao colaborador(a), estagiário ou aprendiz após o nascimento do seu bebê. Para receber, é necessário apresentar a certidão de nascimento ao GPM/Serviço Social de sua unidade. Aproveite esse momento e saiba que desejamos muita saúde para seu bebê.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/kitbebe.webp",
  },
  {
    id: 30,
    categoria: 3,
    nome: "PPR - Prêmio Por Participação Nos Resultados",
    descricao:
      "O colaborador poderá conquistar até dois salários anualmente, conforme o atingimento das metas de sua Unidade/área. ",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/ppr.jpg",
  },
  {
    id: 32,
    categoria: 2,
    nome: "Sesi Educacional",
    descricao:
      "Essa iniciativa visa apoiar e incentivar a educação dos filhos dos nossos colaboradores, possibilitando que eles tenham acesso a uma educação de qualidade.Nessa parceria oferecemos esse benefício, que reembolsará, 50% do valor da mensalidade/matrícula escolar do seu filho matriculado.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/sesieducacional.jpg",
  },
  {
    id: 33,
    categoria: 3,
    nome: "Parceria De Estudo",
    descricao:
      "Quer dar um upgrade no currículo, mas não sabe como?  Conheça nossa parceria de estudo. Com ela, é possível receber ajuda nos cursos de graduação, pós-graduação e especialização. Procure a área de Treinamento e Desenvolvimento de sua Unidade e veja como seguir.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/parceiriasdeestudo.webp",
  },
  {
    id: 34,
    categoria: 3,
    nome: "Moura Clube",
    descricao:
      "O Moura clube é um sistema de reconhecimento que premia os colaboradores conforme critérios específicos com pontuação/volts que permitem que o colaborador efetue trocas conforme itens disponíveis na plataforma. Esse benefício é uma forma de valorizar e motivar os colaboradores, incentivando a excelência no trabalho e reconhecendo os esforços individuais.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/mouraclube.webp",
  },
  {
    id: 36,
    categoria: 4,
    nome: "Gestação Saudável",
    descricao:
      "Programa de acompanhamento das gestantes do Grupo Moura. A assistência é realizada de forma remota, e o objetivo deste acompanhamento no período gestacional é assegurar o desenvolvimento saudável da gestação, permitindo um parto com menores riscos para a mamãe e para o bebê.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/gestacaosaudavel.webp",
  },
  {
    id: 37,
    categoria: 4,
    nome: "Nutricionista",
    descricao:
      "Atendimento clínico nutricional que ocorre de forma presencial nos ambulatórios das Unidades fabris de Belo Jardim decorrente da avaliação de saúde periódica conduzida pelo Serviço Médico. A consulta nutricional consiste em uma completa avaliação incluindo o rastreamento metabólico, seguida pela elaboração do plano alimentar individualizado e funcional de cada colaborador que possuem necessidades nutricionais e distúrbios metabólicos e quando necessário são encaminhados para especialistas da nossa rede credenciada (Assistência Médica Moura), de forma remota e presencial através dos Planos de Saúde disponível para cada localidade.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/nutricionista.webp",
  },
  {
    id: 39,
    categoria: 2,
    nome: "Social Cuidando De Você",
    descricao:
      "Programa de suporte ao colaborador conduzido pela área Social do GPM cujo objetivo é fazer uma leitura crítica da realidade a qual ele está inserido, identificando motivos que causam suas expressões sociais apresentadas no trabalho e assim intervir com encaminhamentos para os suportes que a empresa dispõe ou para suporte externo conforme necessidade.  ",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/socialcuidando.webp",
  },
  {
    id: 40,
    categoria: 2,
    nome: "Assistência Psicológica",
    descricao:
      "Suporte psicológico presencial nas Unidades de Belo Jardim. É um atendimento de acolhimento breve com intervenções pontuais e quando necessário ocorre o encaminhamento para a rede credenciada a nossa Assistência Médica ou Planos de Saúde disponível para cada localidade.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/assistenciapsicologica.webp",
  },
  {
    id: 42,
    categoria: 2,
    nome: "Programa Vida Na Maturidade",
    descricao:
      "É um programa voltado para o planejamento da aposentadoria dos colaboradores, onde é realizado entrevistas junto ao advogado previdenciário. Critérios para participar do programa: Ter, no mínimo, 45 anos de idade e 25 anos de contribuição; Colaboradores que já realizaram entrevistas podem participar novamente; Público Administrativo e industrial das unidades, 01, 02, 04, 05, 08, 10, 12, ITEMM, Insituto Conceição Moura, MBAI, Metalúrgica Bitury, RM Participações e Tranportadora Bitury; As reuniões acontecem mensalmente. O GPM entrará em contato com os inscritos para informar a data, horário e local.",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/vidanamaturidade.webp",
  },
  {
    id: 44,
    categoria: 3,
    nome: "TotalPass",
    descricao:
      "Bem-vindo ao TotalPass! Aqui você será direcionado para um aplicativo voltado para sua saúde e bem-estar, bem como de seus dependentes. Com uma vasta rede de academias credenciadas, o TotalPAss é um aliado poderoso na busca pela qualidade de vida. Com ele é possível encontrar uma academia perfeita, que combina com seu estilo e suas necessidades. Acesse https://totalpass.com.br",
    urlImage:
      "https://beneficiosmourastorage.blob.core.windows.net/content-beneficios-moura/totalpass.webp",
  },
];

// URL do servidor local
const serverURL = "http://localhost:5218/beneficios";

// Função para fazer a solicitação POST
async function postBeneficios(beneficios) {
  try {
    for (const beneficio of beneficios) {
      // Extrai os campos Categoria, Nome, Descricao e urlImage do objeto beneficio
      const { categoria, nome, descricao, urlImage } = beneficio;

      // Cria o objeto com os dados a serem enviados no POST
      const data = {
        Categoria: categoria,
        Nome: nome,
        Descricao: descricao,
        urlImage: urlImage,
      };

      // Faz a solicitação POST para o servidor local
      await axios.post(serverURL, data);

      console.log(`Benefício "${nome}" enviado com sucesso.`);
    }
  } catch (error) {
    console.error("Erro ao enviar benefícios:", error);
  }
}

// Chama a função para enviar os benefícios
postBeneficios(beneficios);
