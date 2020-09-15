<template>
  <validation-observer prefix="observer" v-slot="{ handleSubmit }">
    <div v-if="!loaded" class="d-flex justify-content-center">
      <b-spinner class="m-5 text-primary"></b-spinner>
    </div>
    <b-form @submit.stop.prevent="handleSubmit(onSubmit)" v-if="loaded">
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
              <validation-provider v-slot="validationContext" vid="japaneseName" immediate rules="required_if_not:englishName" :custom-messages="{ required_if_not: 'A Japanese or English name is required.' }">
                <b-form-group label="Japanese Name">
                  <b-form-input v-model="anime.japaneseName" :state="getValidationState(validationContext)" />
                  <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
                </b-form-group>
              </validation-provider>

              <validation-provider v-slot="validationContext" vid="englishName" immediate rules="required_if_not:japaneseName" :custom-messages="{ required_if_not: 'A Japanese or English name is required.' }">
                <b-form-group label="English Name">
                  <b-form-input v-model="anime.englishName" :state="getValidationState(validationContext)" />
                  <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
                </b-form-group>
              </validation-provider>
            </div>
          </div>

          <validation-provider v-slot="validationContext">
            <b-form-group label="Synopsys">
              <b-form-textarea v-model="anime.synopsys" rows="5"></b-form-textarea>
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>
          
          <validation-provider v-slot="validationContext">
            <b-form-group label="Background">
              <b-form-textarea v-model="anime.background" rows="5"></b-form-textarea>
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>
          
          <validation-provider v-slot="validationContext">
            <b-form-group label="Source">
              <b-form-input type="text" v-model="anime.source" />
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>
        </div>

        <div class="col-md-6">
          <validation-provider v-slot="validationContext">
            <b-form-group label="Broadcast Time">
              <b-form-timepicker type="time" v-model="anime.broadcastTime" />
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>

          <validation-provider v-slot="validationContext">
            <b-form-group label="Start Airing Date">
              <b-form-datepicker v-model="anime.startAirDate"></b-form-datepicker>
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>

          <validation-provider v-slot="validationContext">
            <b-form-group label="End Airing Date">
              <b-form-datepicker type="date" v-model="anime.endAirDate" />
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>

          <validation-provider v-slot="validationContext">
            <b-form-group label="Number of Episodes">
              <b-form-input type="number" v-model.number="anime.episodeCount" />
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>

          <validation-provider v-slot="validationContext">
            <b-form-group label="Show Type">
              <b-form-select type="number" v-model="anime.showType"></b-form-select>
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>

          <validation-provider v-slot="validationContext">
            <b-form-group label="TV Rating">
              <b-form-select type="number" v-model="anime.tvRating"></b-form-select>
              <b-form-invalid-feedback> {{ validationContext.errors[0] }}</b-form-invalid-feedback>
            </b-form-group>
          </validation-provider>
        </div>
      </div>

      <button class="btn btn-primary">Save</button>
    </b-form>
  </validation-observer>
</template>

<script>

export default {
  name: 'edit-anime',
  data() {
    return {
      anime: {},
      loaded: false
    };
  },
  async mounted() {
    const id = this.$route.params.id;
    const response = await this.axios.get(`/anime/${id}/edit`);
    if (response.data.id !== id)
      return;

    this.anime = response.data;
    this.loaded = true;
  },
  computed: {
    imagePreview() {
      return this.anime.imageUrl || '/images/no-icon.svg';
    }
  },
  methods: {
    getValidationState({ dirty, validated, valid = null }) {
      return dirty || validated ? valid : null;
    },
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
    onSubmit() {
      let formData = new FormData();
      Object.keys(this.anime).forEach(key => {
        if (this.anime[key] !== null) {
          formData.append(key, this.anime[key]);
        }
      });

      this.axios.put(`/anime/${this.anime.id}`, formData)
        .then(response => {
          this.$router.push({ name: 'view-anime', params: { id: this.anime.id } });
        }).catch(json => {
          if (json.errors) {
            this.$refs.form.setErrors(json.errors);
          }
        });
    }
  }
}
</script>
