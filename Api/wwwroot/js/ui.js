function setUILoading(loading) {
    isLoading = loading;
    var btnP = document.getElementById('btnPesquisar');
    btnP.disabled = loading;
    btnP.innerHTML = loading
        ? '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>&nbsp;Carregando...'
        : '<i class="bi bi-arrow-clockwise"></i> Atualizar';
    document.getElementById('btnCadastrar').disabled = loading;
    if (loading) setExportButton(false);
    toggleTableOverlay(loading);
}

function toggleTableOverlay(show) {
    var overlay = document.getElementById('tblLoadingOverlay');
    if (show) {
        overlay.classList.add('active');
        renderSkeletonRows();
    } else {
        overlay.classList.remove('active');
    }
}

function renderSkeletonRows() {
    var widths = [7, 14, 5, 6, 3, 3, 6, 7, 5, 4];
    var rows = '';
    for (var i = 0; i < 6; i++) {
        var cells = widths.map(function (w) {
            return '<td><span class="skeleton-bar" style="width:' + (w * 10) + 'px;"></span></td>';
        }).join('');
        rows += '<tr class="skeleton-row">' + cells + '</tr>';
    }
    document.getElementById('tblBody').innerHTML = rows;
}

function setExportButton(enabled) {
    document.getElementById('btnExportar').disabled = !enabled;
}