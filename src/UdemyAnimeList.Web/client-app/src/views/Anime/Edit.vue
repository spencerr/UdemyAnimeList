<template>
  <form class="container-fluid needs-validation" @submit="formSubmit">
    <div id="validationSummary" class="alert alert-danger d-none"></div>
    <div class="row">
      <div class="col-md-6">
        <div class="row">
          <div class="col-md-4 text-center align-self-center">
            <button id="imageSelect" type="button" @click="openFilePicker">
              <img id="imagePreview" :src="imagePreview" class="preview-icon" />
            </button>
            <input id="image" class="d-none" type="file" accept="image/x-png,image/jpeg" @change="processFile($event)" />
          </div>
          <div class="col-md-8">
            <div class="form-group">
              <label for="japaneseName">Japanese Name</label>
              <b-form-input v-model="anime.japaneseName" data-val="true" data-val-require-one-of="A Japanese or English name is required." data-require-one-of-element="EnglishName" />
            </div>
            <div class="form-group">
              <label for="englishName">English NAme</label>
              <b-form-input v-model="anime.englishName" data-val="true" data-val-require-one-of="A Japanese or English name is required." data-require-one-of-element="JapaneseName" />
            </div>
          </div>
        </div>

        <div class="form-group">
          <label for="synopsys">Synopsys</label>
          <b-form-textarea v-model="anime.synopsys" rows="5"></b-form-textarea>
        </div>

        <div class="form-group">
          <label for="background">Background</label>
          <b-form-textarea v-model="anime.background" rows="5"></b-form-textarea>
        </div>

        <div class="form-group">
          <label for="source">Source</label>
          <b-form-input type="text" v-model="anime.source" />
        </div>
      </div>

      <div class="col-md-6">
        <div class="form-group">
          <label for="broadcastTime">Broadcast Time</label>
          <b-form-timepicker type="time" v-model="anime.broadcastTime" />
        </div>

        <div class="form-group">
          <label for="startAirDate">Start Airing Date</label>
          <b-form-datepicker v-model="anime.startAirDate"></b-form-datepicker>
        </div>

        <div class="form-group">
          <label for="endAirDate">End Airing Date</label>
          <b-form-datepicker type="date" v-model="anime.endAirDate" />
        </div>

        <div class="form-group">
          <label for="episodeCount">Number of Episodes</label>
          <b-form-input type="number" v-model.number="anime.episodeCount" />
        </div>

        <div class="form-group">
          <label for="showType">Show Type</label>
          <b-form-select type="number" v-model="anime.showType"></b-form-select>
        </div>

        <div class="form-group">
          <label for="tvRating">TV Rating</label>
          <b-form-select type="number" v-model="anime.tvRating"></b-form-select>
        </div>
      </div>
    </div>

    <button class="btn btn-primary">Save</button>
  </form>
</template>

<script>
  export default {
    name: 'edit-anime',
    data() {
      return {
        anime: {}
      };
    },
    mounted() {
      const id = this.$route.params.id;
      this.axios.get(`/anime/${id}/edit`).then(response => {
        if (response.data.id !== id) return;
        this.anime = response.data;
      })
    },
    computed: {
      imagePreview() {
        return this.anime.imageUrl || '/images/no-icon.svg';
      }
    },
    methods: {
      processFile(event) {
        this.anime.image = event.target.files[0];
        var reader = new FileReader();
        reader.onload = function (e) {
          document.getElementById('imagePreview').setAttribute('src', e.target.result);
        };

        reader.readAsDataURL(this.anime.image);
      },
      openFilePicker() {
        document.getElementById('image').click();
      },
      formSubmit(e) {
        e.preventDefault();
        this.axios.put(`/anime/${this.anime.id}`, this.anime).then(response => {
          this.$router.push({ name: 'view-anime', params: { id: this.anime.id } });
        }).catch(error => {
          console.log(error);
        });
      }
    }
  }
</script>
