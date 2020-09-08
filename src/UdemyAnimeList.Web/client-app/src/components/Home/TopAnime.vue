<template>
  <div class="row no-gutters">
    <div class="col-4">
      <img class="card-img" :src="anime.imageUrl" />
    </div>
    <div class="col-8">
      <div class="card-body pt-4">
        <h6>{{ anime.rank }}: <router-link :to="{ name: 'view-anime', params: { id: anime.id } }">{{ anime.name }}</router-link></h6>
        <p class="card-text text-muted">{{ anime.showType }}, {{ anime.episodeCount }} eps, scored {{ anime.score }} <br />{{ anime.membersLiked }} members</p>
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    props: ['anime']
  }
</script>
