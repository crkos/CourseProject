var templateId = "@Model.Template.Id";
const easyMDE = new EasyMDE({
    element: document.getElementById('my-text-area'),
    minHeight: "400px",
    autosave: {
        enabled: true,
        uniqueId: templateId,
        delay: 1000,
        submit_delay: 5000,
        timeFormat: {
            locale: 'en-US',
            format: {
                year: 'numeric',
                month: 'long',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
            },
        },
        text: "Autosaved locally: "
    },
    uploadImage: true,
    imageMaxSize: 1024 * 1024 * 5,
    imageUploadEndpoint: "/Template/UploadImage",
    imagePathAbsolute: true,
});