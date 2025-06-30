<template>
    <div class="dataTables_paginate paging_simple_numbers">
        <nav aria-label="Page navigation">
            <ul class="pagination pagination-rounded">
                <li class="page-item first" @click="goToPage(1)" :class="{ disabled: pageNo === 1 }">
                    <a class="page-link" href="#"><i class="ti ti-chevrons-left ti-sm"></i></a>
                </li>
                <li class="page-item prev" @click="goToPage(pageNo - 1)" :class="{ disabled: pageNo === 1 }">
                    <a class="page-link" href="#"><i class="ti ti-chevron-left ti-sm"></i></a>
                </li>
                <li class="page-item" :class="{ active: pageNo === page }" v-if="pageNo > 1">
                    <a class="page-link" href="#" @click.prevent="goToPage(pageNo - 1)">{{ pageNo - 1 }}</a>
                </li>
                <li class="page-item active">
                    <a class="page-link" href="#">{{ pageNo }}</a>
                </li>
                <li class="page-item" v-if="pageNo < totalPages">
                    <a class="page-link" href="#" @click.prevent="goToPage(pageNo + 1)">{{ pageNo + 1 }}</a>
                </li>
                <li class="page-item next" @click="goToPage(pageNo + 1)" :class="{ disabled: pageNo === totalPages }">
                    <a class="page-link" href="#"><i class="ti ti-chevron-right ti-sm"></i></a>
                </li>
                <li class="page-item last" @click="goToPage(totalPages)" :class="{ disabled: pageNo === totalPages }">
                    <a class="page-link" href="#"><i class="ti ti-chevrons-right ti-sm"></i></a>
                </li>
            </ul>
        </nav>
    </div>
</template>

<script>
    export default {
        props: {
            pageNo: {
                type: Number,
                required: true
            },
            pageSize: {
                type: Number,
                required: true
            },
            totalItems: {
                type: Number,
                required: true
            }
        },
        computed: {
            totalPages() {
                return Math.ceil(this.totalItems / this.pageSize);
            }
        },
        methods: {
            goToPage(page) {
                if (page >= 1 && page <= this.totalPages) {
                    this.$emit('page-changed', page);
                }
            }
        }
    }
</script>

<style scoped>
    .pagination .disabled {
        pointer-events: none;
        opacity: 0.5;
    }

    .pagination .active .page-link {
        font-weight: bold;
    }
</style>