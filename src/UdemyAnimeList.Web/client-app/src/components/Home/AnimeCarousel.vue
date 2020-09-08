<template>
  <li>
    <router-link :to="{ name: 'view-anime', params: { id: anime.id } }">
      <span class="position-absolute">{{ anime.name }}</span>
      <img :src="anime.imageUrl" style="width: 100%;" />
    </router-link>
  </li>
</template>

<script>
  export default {
    props: ['anime']
  }
</script>
