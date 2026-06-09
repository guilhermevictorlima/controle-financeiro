function parseDotNetDate(val) {
    if (!val) return null;
    if (typeof val === 'string') {
        var m = val.match(/\/Date\((-?\d+)(?:[+-]\d{4})?\)\//);
        if (m) return new Date(parseInt(m[1], 10));
    }
    var d = new Date(val);
    return isNaN(d) ? null : d;
}

function formatDate(val) {
    var d = parseDotNetDate(val);
    if (!d) return '—';
    return d.toLocaleDateString('pt-BR');
}

function formatCurrency(value) {
    return (value || 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

function formatPercent(value) {
    return (value || 0).toLocaleString('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }) + '%';
}

function escapeHtml(str) {
    var d = document.createElement('div');
    d.appendChild(document.createTextNode(str || ''));
    return d.innerHTML;
}

function parseBrDecimal(str) {
    if (!str) return 0;
    return parseFloat(str.replace(/\./g, '').replace(',', '.')) || 0;
}

function formatBrDecimal(val) {
    return val.toLocaleString('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

function isCredito(tipo) {
    return tipo === 0 || tipo === 'Credito';
}

function isPago(status) {
    return status === 1 || status === 'Pago';
}

var STATUS_MAP = {
    0: { cls: 'badge-aberto', lbl: 'Aberto' },
    1: { cls: 'badge-pago', lbl: 'Pago' },
    2: { cls: 'badge-cancelado', lbl: 'Cancelado' },
    'Aberto': { cls: 'badge-aberto', lbl: 'Aberto' },
    'Pago': { cls: 'badge-pago', lbl: 'Pago' },
    'Cancelado': { cls: 'badge-cancelado', lbl: 'Cancelado' }
};