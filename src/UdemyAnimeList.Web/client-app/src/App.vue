<template>
  <div id="app">
    <header>
      <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white box-shadow mb-3">
        <div class="container">
          <router-link to="/" class="navbar-brand">UdemyAnimeList</router-link>
          <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                  aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
            <ul class="navbar-nav flex-grow-1">
              <li class="nav-item">
                <router-link to="/" class="navbar-brand">Home</router-link>
              </li>
              <li class="nav-item">
                <router-link to="/privacy" class="navbar-brand">Privacy</router-link>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>

    <div class="container">
      <main role="main" class="pb-3">
        <router-view />
      </main>
    </div>

    <footer class="border-top footer text-muted">
      <div class="container">
        &copy; 2020 - UdemyAnimeList - <router-link to="/privacy">Privacy</router-link>
      </div>
    </footer>
  </div>
</template>
