declare module '@/plugins/casl/composables/useAbility' {
  import type { AnyObject } from 'vue'
  export function useAbility(): { update: (rules: AnyObject[]) => void }
  export default function useAbility(): { update: (rules: AnyObject[]) => void }
}

declare module '@/plugins/casl/ability' {
  export const ability: { update: (rules: any[]) => void }
  export function reloadAbilityFromCookie(): void
}

