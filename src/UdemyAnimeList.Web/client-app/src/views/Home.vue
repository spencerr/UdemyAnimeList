<template>
  <div class="container-fluid">
    <div class="row">
      <div class="col-7">
        <div class="carousel">
          <p>{{ season}} Anime <router-link to="/anime/current-season" class="float-right">View More</router-link></p>
          <ul class="anime-carousel">
            <AnimeCarousel v-for="anime in currentSeasonAnime" :anime="anime"></AnimeCarousel>
          </ul>
        </div>

        <div class="carousel">
          <ul class="anime-carousel">
            <AnimeCarousel v-for="anime in recentlyUpdatedAnime" :anime="anime"></AnimeCarousel>
          </ul>
        </div>
      </div>
      <div class="col-5">
        <div class="card mb-2">
          <div class="card-header">
            <span>Top Airing Anime</span><a asp-controller="Animes" asp-action="TopAiring" class="float-right">More</a>
          </div>
          <div class="card-body">
            <PopularAnime v-for="anime in topAiringAnime" :anime="anime"></PopularAnime>
          </div>
        </div>

        <div class="card mb-2">
          <div class="card-header">
            <span>Top Upcoming Anime</span><a asp-controller="Animes" asp-action="TopUpcoming" class="float-right">More</a>
          </div>
          <div class="card-body">
            <PopularAnime v-for="anime in topUpcomingAnime" :anime="anime"></PopularAnime>
          </div>
        </div>

        <div class="card mb-2">
          <div class="card-header">
            <span>Most Popular Anime</span><a asp-controller="Animes" asp-action="MostPopular" class="float-right">More</a>
          </div>
          <div class="card-body">
            <PopularAnime v-for="anime in mostPopularAnime" :anime="anime"></PopularAnime>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
  import PopularAnime from '@/components/Home/TopAnime.vue'
  import AnimeCarousel from '@/components/Home/AnimeCarousel.vue'

  export default {
    name: 'Home',
    components: {
      PopularAnime,
      AnimeCarousel
    },
    data() {
      return {
        currentSeason: {},
        currentSeasonAnime: [],
        recentlyUpdatedAnime: [],
        topAiringAnime: [],
        topUpcomingAnime: [],
        mostPopularAnime: []
      };
    },
    computed: {
      season() {
        return `${this.currentSeason?.airingSeason || ''} ${this.currentSeason?.year | new Date().getFullYear()}`;
      }
    },
    mounted() {
      this.axios.get('/home/retrieve').then(response => {
        this.currentSeason = response.data.currentSeason;
        this.currentSeasonAnime = response.data.currentSeasonAnime;
        this.recentlyUpdatedAnime = response.data.recentlyUpdatedAnime;
        this.topAiringAnime = response.data.topAiringAnime;
        this.topUpcomingAnime = response.data.topUpcomingAnime;
        this.mostPopularAnime = response.data.mostPopularAnime;
      })
    }
  }
</script>
