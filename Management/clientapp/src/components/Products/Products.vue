<template>
    <div>
        <div class="content-wrapper">
            <div class="content-header">
                <div class="container-fluid">
                    <div class="row mb-2">
                        <div class="col-sm-6">
                            <h1 class="m-0">إدارة المواد</h1>
                        </div>
                        <div class="col-sm-6">
                            <ol class="breadcrumb float-sm-right">
                                <li class="breadcrumb-item"><a href="#">الرئيسية</a></li>
                                <li class="breadcrumb-item active">المواد</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>

            <div class="content">
                <div class="container-fluid">
                    <!-- Filters Card -->
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">البحث والفلترة</h3>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>البحث</label>
                                        <input type="text" class="form-control" v-model="searchTerm" placeholder="ابحث في اسم المادة أو الوصف...">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>عدد العناصر في الصفحة</label>
                                        <select class="form-control" v-model="pageSize">
                                            <option value="10">10</option>
                                            <option value="20">20</option>
                                            <option value="50">50</option>
                                            <option value="100">100</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>&nbsp;</label>
                                        <div>
                                            <button class="btn btn-primary" @click="loadProducts">
                                                <i class="fas fa-search"></i> بحث
                                            </button>
                                            <button class="btn btn-secondary" @click="resetFilters">
                                                <i class="fas fa-undo"></i> إعادة تعيين
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Products Table Card -->
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">قائمة المواد</h3>
                            <div class="card-tools">
                                <button class="btn btn-success" @click="openAddDialog">
                                    <i class="fas fa-plus"></i> إضافة مادة جديدة
                                </button>
                            </div>
                        </div>
                        <div class="card-body">
                            <div v-if="loading" class="text-center">
                                <i class="fas fa-spinner fa-spin fa-2x"></i>
                                <p>جاري التحميل...</p>
                            </div>
                            <div v-else>
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>الاسم</th>
                                                <th>الوصف</th>
                                                <th>الإجراءات</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="product in products" :key="product.id">
                                                <td>{{ product.name }}</td>
                                                <td>{{ product.description }}</td>
                                                <td>
                                                    <button class="btn btn-sm btn-info" @click="openEditDialog(product)">
                                                        <i class="fas fa-edit"></i> تعديل
                                                    </button>
                                                    <button class="btn btn-sm btn-danger" @click="deleteProduct(product.id)">
                                                        <i class="fas fa-trash"></i> حذف
                                                    </button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                <!-- Pagination -->
                                <div class="d-flex justify-content-between align-items-center mt-3">
                                    <div>
                                        <span>عرض {{ startIndex + 1 }} إلى {{ endIndex }} من {{ totalCount }} نتيجة</span>
                                    </div>
                                    <div>
                                        <button class="btn btn-sm btn-outline-primary" @click="previousPage" :disabled="currentPage === 1">
                                            <i class="fas fa-chevron-right"></i> السابق
                                        </button>
                                        <span class="mx-2">صفحة {{ currentPage }} من {{ totalPages }}</span>
                                        <button class="btn btn-sm btn-outline-primary" @click="nextPage" :disabled="currentPage === totalPages">
                                            التالي <i class="fas fa-chevron-left"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Add/Edit Product Modal -->
        <div class="modal fade" id="productModal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">{{ isEditing ? 'تعديل مادة' : 'إضافة مادة جديدة' }}</h5>
                        <button type="button" class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <form @submit.prevent="saveProduct">
                            <div class="form-group">
                                <label>اسم المادة</label>
                                <input type="text" class="form-control" v-model="productForm.name" required>
                            </div>
                            <div class="form-group">
                                <label>الوصف</label>
                                <textarea class="form-control" v-model="productForm.description" rows="3"></textarea>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">إلغاء</button>
                        <button type="button" class="btn btn-primary" @click="saveProduct" :disabled="saving">
                            <i v-if="saving" class="fas fa-spinner fa-spin"></i>
                            {{ saving ? 'جاري الحفظ...' : 'حفظ' }}
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import DataService from '../../Shared/DataService';

export default {
    name: 'Products',
    data() {
        return {
            products: [],
            loading: false,
            saving: false,
            searchTerm: '',
            currentPage: 1,
            pageSize: 10,
            totalCount: 0,
            isEditing: false,
            productForm: {
                id: null,
                name: '',
                description: ''
            }
        };
    },
    computed: {
        totalPages() {
            return Math.ceil(this.totalCount / this.pageSize);
        },
        startIndex() {
            return (this.currentPage - 1) * this.pageSize;
        },
        endIndex() {
            return Math.min(this.startIndex + this.pageSize, this.totalCount);
        }
    },
    methods: {
        async loadProducts() {
            this.loading = true;
            try {
                const params = {
                    PageNumber: this.currentPage,
                    PageSize: this.pageSize,
                    SearchTerm: this.searchTerm
                };
                
                const response = await DataService.getProducts(params);
                if (response && response.data) {
                    this.products = response.data.items || [];
                    this.totalCount = response.data.totalCount || 0;
                }
            } catch (error) {
                console.error('Error loading products:', error);
                this.$message.error('حدث خطأ أثناء تحميل المواد');
            } finally {
                this.loading = false;
            }
        },
        
        resetFilters() {
            this.searchTerm = '';
            this.currentPage = 1;
            this.loadProducts();
        },
        
        previousPage() {
            if (this.currentPage > 1) {
                this.currentPage--;
                this.loadProducts();
            }
        },
        
        nextPage() {
            if (this.currentPage < this.totalPages) {
                this.currentPage++;
                this.loadProducts();
            }
        },
        
        openAddDialog() {
            this.isEditing = false;
            this.productForm = {
                id: null,
                name: '',
                description: ''
            };
            $('#productModal').modal('show');
        },
        
        openEditDialog(product) {
            this.isEditing = true;
            this.productForm = {
                id: product.id,
                name: product.name,
                description: product.description
            };
            $('#productModal').modal('show');
        },
        
        async saveProduct() {
            this.saving = true;
            try {
                if (this.isEditing) {
                    await DataService.updateProduct(this.productForm.id, this.productForm);
                    this.$message.success('تم تحديث المادة بنجاح');
                } else {
                    await DataService.createProduct(this.productForm);
                    this.$message.success('تم إضافة المادة بنجاح');
                }
                
                $('#productModal').modal('hide');
                this.loadProducts();
            } catch (error) {
                console.error('Error saving product:', error);
                this.$message.error('حدث خطأ أثناء حفظ المادة');
            } finally {
                this.saving = false;
            }
        },
        
        async deleteProduct(id) {
            if (confirm('هل أنت متأكد من حذف هذه المادة؟')) {
                try {
                    await DataService.deleteProduct(id);
                    this.$message.success('تم حذف المادة بنجاح');
                    this.loadProducts();
                } catch (error) {
                    console.error('Error deleting product:', error);
                    this.$message.error('حدث خطأ أثناء حذف المادة');
                }
            }
        }
    },
    
    mounted() {
        this.loadProducts();
    }
};
</script>

<style scoped>
.content-wrapper {
    min-height: 100vh;
    background-color: #f4f6f9;
}

.content-header {
    background-color: #fff;
    border-bottom: 1px solid #dee2e6;
    padding: 15px 0;
}

.content {
    padding: 20px 0;
}

.card {
    margin-bottom: 20px;
    box-shadow: 0 0 1px rgba(0,0,0,.125), 0 1px 3px rgba(0,0,0,.2);
}

.card-header {
    background-color: #f8f9fa;
    border-bottom: 1px solid #dee2e6;
}

.table th {
    background-color: #f8f9fa;
    border-top: none;
}

.btn-sm {
    margin: 0 2px;
}

.modal-header {
    background-color: #f8f9fa;
    border-bottom: 1px solid #dee2e6;
}

.modal-footer {
    background-color: #f8f9fa;
    border-top: 1px solid #dee2e6;
}
</style> 