CREATE TABLE contacorrente (
    idcontacorrente VARCHAR2(37) PRIMARY KEY, -- id da conta corrente
    numero NUMBER(10) NOT NULL UNIQUE, -- numero da conta corrente
    nome VARCHAR2(100) NOT NULL, -- nome do titular da conta corrente
    ativo NUMBER(1) DEFAULT 0 NOT NULL CHECK (ativo in (0,1)), -- indicativo se a conta esta ativa. (0 = inativa, 1 = ativa).
    senha VARCHAR2(100) NOT NULL,
    salt VARCHAR2(100) NOT NULL,
    cpf VARCHAR2(11) NOT NULL UNIQUE
)
/
CREATE TABLE movimento (
	idmovimento VARCHAR2(37) PRIMARY KEY, -- identificacao unica do movimento
	idcontacorrente VARCHAR2(37) NOT NULL, -- identificacao unica da conta corrente
	datamovimento VARCHAR2(25) NOT NULL, -- data do movimento no formato DD/MM/YYYY
	tipomovimento VARCHAR2(1) NOT NULL CHECK (tipomovimento in ('C','D')), -- tipo do movimento. (C = Credito, D = Debito).
	valor NUMBER(10,2) NOT NULL, -- valor do movimento. Usar duas casas decimais.
	CONSTRAINT fk_contacorrente_movimento FOREIGN KEY (idcontacorrente) REFERENCES contacorrente(idcontacorrente)
)
/
CREATE TABLE idempotencia (
	chave_idempotencia VARCHAR2(37) PRIMARY KEY, -- identificacao chave de idempotencia
	requisicao VARCHAR2(1000), -- dados de requisicao
	resultado VARCHAR2(1000) -- dados de retorno
)
