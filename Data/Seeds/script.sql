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