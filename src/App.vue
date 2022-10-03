<template>
<Header/>
<div class="main">
   <Menu/>
<Container/>
</div>

</template>

<script>
import Header from './components/layouts/TheHeader/TheHeader.vue'
import Menu from './components/layouts/TheMenu/TheMenu.vue'
import Container from './components/layouts/TheContainer/TheContainer.vue'
export default {
  name: 'App',
  components: {
    Header,
    Menu,
    Container
  }
}
</script>
<style>
  @import url(./css/main.css);
  </style>
