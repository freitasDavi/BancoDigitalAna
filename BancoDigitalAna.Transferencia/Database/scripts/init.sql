CREATE TABLE transferencia (
	idtransferencia VARCHAR2(37) PRIMARY KEY, -- identificacao unica da transferencia
	idcontacorrente_origem VARCHAR2(37) NOT NULL, -- identificacao unica da conta corrente de origem
	idcontacorrente_destino VARCHAR2(37) NOT NULL, -- identificacao unica da conta corrente de destino
	datamovimento VARCHAR2(25) NOT NULL, -- data do transferencia no formato DD/MM/YYYY
	valor NUMBER(18,2) NOT NULL -- valor da transferencia. Usar duas casas decimais.
)
/
CREATE TABLE idempotencia (
	chave_idempotencia VARCHAR2(37) PRIMARY KEY, -- identificacao chave de idempotencia
	requisicao VARCHAR2(1000), -- dados de requisicao
	resultado VARCHAR2(1000) -- dados de retorno
)