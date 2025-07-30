<script lang="ts" setup>
import VerticalNavSectionTitle from '@/@layouts/components/VerticalNavSectionTitle.vue'
import VerticalNavGroup from '@layouts/components/VerticalNavGroup.vue'
import VerticalNavLink from '@layouts/components/VerticalNavLink.vue'

// استيراد عناصر التنقل
import navItems from '@/navigation/vertical'

// طباعة العناصر للتأكد من تحميلها
console.log('🔍 Nav Items:', navItems)
</script>

<template>
  <!-- عرض عناصر التنقل للتأكد من عملها -->
  <div>
    <h3>Debug Navigation Items</h3>
    <pre>{{ JSON.stringify(navItems, null, 2) }}</pre>
  </div>

  <!-- عرض العناصر في القائمة الجانبية -->
  <template v-for="item in navItems" :key="item.title">
    <VerticalNavGroup
      v-if="item.children"
      :item="item"
    >
      <VerticalNavLink
        v-for="child in item.children"
        :key="child.title"
        :item="child"
      />
    </VerticalNavGroup>
    
    <VerticalNavLink
      v-else
      :item="item"
    />
  </template>
</template> 