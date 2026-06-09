CREATE TABLE lancamento_financeiro (
    id INT NOT NULL IDENTITY PRIMARY KEY,
    descricao NVARCHAR(250) NOT NULL,
    tipo NVARCHAR(7) NOT NULL, 
    valor_original DECIMAL(18,2) NOT NULL,
    percentual_taxa DECIMAL(5,2) NOT NULL,
    percentual_desconto DECIMAL(5,2) NOT NULL,
    valor_calculado DECIMAL(18,2) NOT NULL,
    data_lancamento DATETIME2 NOT NULL,
    data_criacao DATETIME2 NOT NULL,
    data_pagamento DATETIME2 NULL,
    data_cancelamento DATETIME2 NULL,
    competencia CHAR(7) NOT NULL,
    status NVARCHAR(10) default 'Aberto' NOT NULL
);

insert into lancamento_financeiro (
    descricao,
    tipo,
    valor_original,
    percentual_taxa,
    percentual_desconto,
    valor_calculado,
    data_lancamento,
    data_criacao,
    competencia
)
values (
    'Pagamento de Fornecedor',
    'Debito',
    20000,
    5,
    0,
    21000,
    '2026-06-08',
    '2026-06-08',
    '2026-06'
);


insert into lancamento_financeiro (
    descricao,
    tipo,
    valor_original,
    percentual_taxa,
    percentual_desconto,
    valor_calculado,
    data_lancamento,
    data_criacao,
    competencia
)
values (
    'Retorno Investimento',
    'Credito',
    9500,
    0,
    1.5,
    8075,
    '2026-06-20',
    '2026-06-20',
    '2026-06'
);

