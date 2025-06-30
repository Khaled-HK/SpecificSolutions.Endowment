<template src="./Chat.html">
</template>

<script src="./Chat.js"></script>

<style scoped>
    .sidebar-body {
        max-height: calc(100vh - 150px);
        overflow-y: auto;
    }

        .sidebar-body::-webkit-scrollbar {
            width: 8px;
        }

        .sidebar-body::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        .sidebar-body::-webkit-scrollbar-thumb {
            background: #888;
            border-radius: 4px;
        }

            .sidebar-body::-webkit-scrollbar-thumb:hover {
                background: #555;
            }




    .chat-history-body {
        max-height: calc(100vh - 200px) !important;
        overflow-y: auto !important;
    }

        .chat-history-body::-webkit-scrollbar {
            width: 8px !important;
        }

        .chat-history-body::-webkit-scrollbar-track {
            background: #f1f1f1 !important;
        }

        .chat-history-body::-webkit-scrollbar-thumb {
            background: #888 !important;
            border-radius: 4px !important;
        }

            .chat-history-body::-webkit-scrollbar-thumb:hover {
                background: #555 !important;
            }
</style>
