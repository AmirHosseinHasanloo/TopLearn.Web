function InputCkMaker(tagId) {

    CKEDITOR.replace(tagId, {
        customConfig: '/js/Config.js'
    });
}