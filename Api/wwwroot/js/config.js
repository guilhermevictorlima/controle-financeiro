var API_BASE = '/api/lancamentos-financeiros';

var currentResults = [];
var isLoading = false;
var isSaving = false;
var hadResults = false;
var editandoId = null;
var fpExportCompetencia = null;