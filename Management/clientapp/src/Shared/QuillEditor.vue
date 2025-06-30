<template>
    <div>
        <div id="full-editor"></div>
    </div>
</template>

<script>
    export default {
        name: 'FullQuillEditor',
        props: {
            value: {
                type: String,
                default: ''
            }
        },
        data() {
            return {
                quill: null,
                fullToolbar: [
                    [
                        { font: [] },
                        { size: [] }
                    ],
                    ['bold', 'italic', 'underline', 'strike'],
                    [
                        { color: [] },
                        { background: [] }
                    ],
                    [
                        { script: 'super' },
                        { script: 'sub' }
                    ],
                    [
                        { header: '1' },
                        { header: '2' },
                        'blockquote',
                        'code-block'
                    ],
                    [
                        { list: 'ordered' },
                        { list: 'bullet' },
                        { indent: '-1' },
                        { indent: '+1' }
                    ],
                    [
                        'direction',
                        { align: [] }
                    ],
                    ['link', 'image', 'video', 'formula'],
                    ['clean']
                ]
            };
        },
        mounted() {
            this.loadScripts().then(() => {
                this.initQuill();
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
                    this.loadScript('./assets/vendor/libs/quill/katex.js'),
                    this.loadScript('./assets/vendor/libs/quill/quill.js')
                ]);
            },
            initQuill() {
                const Quill = window.Quill; // Access the Quill global variable
                this.quill = new Quill('#full-editor', {
                    bounds: '#full-editor',
                    placeholder: 'قم بكتابة الوصف ........',
                    modules: {
                        formula: true,
                        toolbar: this.fullToolbar
                    },
                    theme: 'snow'
                });

                // Set initial content
                this.quill.root.innerHTML = this.value;

                // Listen for text changes and emit input event
                this.quill.on('text-change', () => {
                    const html = this.quill.root.innerHTML;
                    this.$emit('input', html);
                });
            }
        },
        watch: {
            value(newValue) {
                // Update the Quill editor content if the prop changes
                if (this.quill.root.innerHTML !== newValue) {
                    this.quill.root.innerHTML = newValue;
                }
            }
        }
    };
</script>

<style scoped>
    /* Add any additional styles here */
</style>