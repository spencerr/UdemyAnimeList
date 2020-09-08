<template>
  <form class="container-fluid needs-validation">
    <div id="validationSummary" class="alert alert-danger d-none"></div>
    <div class="row">
      <div class="col-md-6">
        <div class="row">
          <div class="col-md-4 text-center align-self-center">
            <button id="ImageSelect" type="button">
              <img id="ImagePreview" :src="imagePreview" class="preview-icon" />
            </button>
            <input class="d-none" type="file" accept="image/x-png,image/jpeg" @change="processFile($event)" />
          </div>
          <div class="col-md-8">
            <div class="form-group">
              <label asp-for="JapaneseName"></label>
              <b-form-input v-model="anime.apaneseName" data-val="true" data-val-require-one-of="A Japanese or English name is required." data-require-one-of-element="EnglishName" />
            </div>
            <div class="form-group">
              <label asp-for="EnglishName"></label>
              <b-form-input v-model="anime.englishName" data-val="true" data-val-require-one-of="A Japanese or English name is required." data-require-one-of-element="JapaneseName" />
            </div>
          </div>
        </div>

        <div class="form-group">
          <label asp-for="Synopsys"></label>
          <b-form-textarea v-model="anime.synopsys" rows="5"></b-form-textarea>
        </div>

        <div class="form-group">
          <label asp-for="Background"></label>
          <b-form-textarea v-model="anime.background" rows="5"></b-form-textarea>
        </div>

        <div class="form-group">
          <label asp-for="Source"></label>
          <b-form-input type="text" v-model="anime.source" />
        </div>
      </div>

      <div class="col-md-6">
        <div class="form-group">
          <label asp-for="BroadcastTime"></label>
          <b-form-timepicker type="time" v-model="anime.broadcastTime" />
        </div>

        <div class="form-group">
          <label asp-for="StartAirDate"></label>
          <b-form-datepicker v-model="anime.startAirDate"></b-form-datepicker>
        </div>

        <div class="form-group">
          <label asp-for="EndAirDate"></label>
          <b-form-datepicker type="date" v-model="anime.endAirDate" />
        </div>

        <div class="form-group">
          <label asp-for="EpisodeCount"></label>
          <b-form-input type="number" v-model.number="anime.episodeCount" />
        </div>

        <div class="form-group">
          <label asp-for="ShowType"></label>
          <b-form-select type="number" v-model="anime.showType"></b-form-select>
        </div>

        <div class="form-group">
          <label asp-for="TVRating"></label>
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
      this.axios.get(`/api/anime/${id}/edit`).then(response => {
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
          $('#ImagePreview').attr('src', e.target.result);
        };

        reader.readAsDataURL(this.anime.image);
      }
    }
  }
</script>
