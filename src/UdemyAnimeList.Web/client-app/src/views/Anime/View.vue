<template>
  <div>
    {{ anime.japaneseName }}

    <router-link :to="{ name: 'edit-anime', params: { id: anime.id } }">Edit</router-link>
  </div>
</template>

<script>
  export default {
    name: 'view-anime',
    data() {
      return {
        anime: {}
      };
    },
    mounted() {
      const id = this.$route.params.id;
      this.axios.get(`/api/anime/${id}`).then(response => {
        if (response.data.id !== id) return;
        this.anime = response.data;
      })
    }
  }
</script>
