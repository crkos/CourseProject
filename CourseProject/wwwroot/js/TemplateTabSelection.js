function showQuestionsTab() {
    document.getElementById('questionsTab').classList.remove('d-none');
    document.getElementById('questionsTabBtn').classList.add('active');
    document.getElementById('answersTab').classList.add('d-none');
    document.getElementById('answersTabBtn').classList.remove('active');
    document.getElementById('settingsTab').classList.add('d-none');
    document.getElementById('settingsTabBtn').classList.remove('active');
}

function showAnswersTab() {
    document.getElementById('questionsTab').classList.add('d-none');
    document.getElementById('questionsTabBtn').classList.remove('active');
    document.getElementById('answersTab').classList.remove('d-none');
    document.getElementById('answersTabBtn').classList.add('active');
    document.getElementById('settingsTab').classList.add('d-none');
    document.getElementById('settingsTabBtn').classList.remove('active');
}

function showSettingsTab() {
    document.getElementById('questionsTab').classList.add('d-none');
    document.getElementById('questionsTabBtn').classList.remove('active');
    document.getElementById('answersTab').classList.add('d-none');
    document.getElementById('answersTabBtn').classList.remove('active');
    document.getElementById('settingsTab').classList.remove('d-none');
    document.getElementById('settingsTabBtn').classList.add('active');
}

function copyFillFormToClipboard() {
    navigator.clipboard.writeText(`${window.location.hostname}/form/${templateIdUrl}/fill`);
    toastr.success("Copied to clipboard.");
}