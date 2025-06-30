<!-- To call this file you must add this varible


    base64Image: ''


    import FileUpload from '../../Shared/FileUpload.vue';

    components: {
        FileUpload
    },


    //Method
    //handleFileUpload(base64String) {
    //    this.base64Image = base64String;
    //}


    -->
<template>
    <form action="/upload"
          class="dropzone needsclick"
          id="dropzone-multi"
          @submit.prevent>
        <div class="dz-message needsclick">
            Drop files here or click to upload
            <span class="note needsclick">(This is just a demo dropzone. Selected files are <span class="fw-medium">not</span> actually uploaded.)</span>
        </div>
        <div class="fallback">
            <input name="file" type="file" @change="handleFileChange" />
        </div>
    </form>
</template>

<script>
    export default {
        name: 'FileUpload',
        mounted() {
            this.loadScripts().then(() => {
                this.initDropzone();
            });
        },
        methods: {
            loadScript(src) {
                return new Promise((resolve, reject) => {
                    const script = document.createElement('script');
                    script.src = src;
                    script.onload = () => resolve();
                    script.onerror = () => reject(new Error(`Failed to load script: ${src}`));
                    document.head.appendChild(script);
                });
            },
            loadScripts() {
                return Promise.all([
                    this.loadScript('./assets/vendor/libs/dropzone/dropzone.js')
                ]);
            },
            initDropzone() {
                if (typeof window.Dropzone === 'undefined') {
                    console.error('Dropzone library not loaded.');
                    return;
                }

                // Disable autoDiscover
                window.Dropzone.autoDiscover = false;

                // Create a new Dropzone instance
                const dropzoneMulti = new window.Dropzone('#dropzone-multi', {
                    previewTemplate: this.getPreviewTemplate(),
                    parallelUploads: 1,
                    maxFilesize: 5, // Maximum file size in MB
                    addRemoveLinks: true,
                    init: () => {
                        dropzoneMulti.on('addedfile', (file) => {
                            this.toBase64(file); // Convert file to Base64 if needed
                        });
                        dropzoneMulti.on('removedfile', (file) => {
                            // Handle file removal if necessary
                            console.log(`${file.name} has been removed.`);
                        });
                    }
                });
            },
            getPreviewTemplate() {
                // Define a preview template
                return `
        <div class="dz-preview dz-file-preview">

          <div class="dz-thumbnail">
           <div class="dz-image"><img data-dz-thumbnail /></div>
           <div class="dz-progress"><span class="dz-upload" data-dz-uploadprogress></span></div>
           <div class="dz-success-mark"><span>✔</span></div>
           <div class="dz-error-mark"><span>✖</span></div>
          </div>
          <div class="dz-filename"><span data-dz-name></span></div>
           <div class="dz-size"><span data-dz-size></span></div>
        </div>
      `;
            },
            handleFileChange(event) {
                const files = event.target.files;
                if (files.length > 0) {
                    this.toBase64(files[0]);
                }
            },
            toBase64(file) {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => {
                    const base64String = reader.result;
                    this.$emit('file-uploaded', base64String);
                };
                reader.onerror = (error) => {
                    console.error('Error: ', error);
                };
            }
        }
    };
</script>

