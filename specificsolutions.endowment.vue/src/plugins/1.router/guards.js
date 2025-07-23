export const setupGuards = router => {
  // 👉 router.beforeEach
  // Docs: https://router.vuejs.org/guide/advanced/navigation-guards.html#global-before-guards
  router.beforeEach((to, from, next) => {
    try {
      /*
       * If it's a public route, continue navigation. This kind of pages are allowed to visited by login & non-login users. Basically, without any restrictions.
       * Examples of public routes are, 404, under maintenance, etc.
       */
      if (to.meta.public) {
        next()
        return
      }

      /**
       * Check if user is logged in by checking if token & user data exists in local storage
       * Feel free to update this logic to suit your needs
       */
      const userData = useCookie('userData')
      const accessToken = useCookie('accessToken')
      const isLoggedIn = !!(userData.value && accessToken.value)

      /*
        If user is logged in and is trying to access login like page, redirect to home
        else allow visiting the page
        (WARN: Don't allow executing further by return statement because next code will check for permissions)
       */
      if (to.meta.unauthenticatedOnly) {
        if (isLoggedIn) {
          next('/')
          return
        } else {
          next()
          return
        }
      }

      // If user is not logged in and trying to access protected route
      if (!isLoggedIn) {
        next({
          name: 'login',
          query: {
            ...to.query,
            to: to.fullPath !== '/' ? to.path : undefined,
          },
        })
        return
      }

      // User is logged in, allow access
      next()
    } catch (error) {
      console.error('Navigation guard error:', error)
      // Fallback to login page
      next({
        name: 'login',
        query: {
          ...to.query,
          to: to.fullPath !== '/' ? to.path : undefined,
        },
      })
    }
  })
}
