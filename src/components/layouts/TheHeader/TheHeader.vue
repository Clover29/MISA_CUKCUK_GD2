<template>
  <div class="header">
    <div class="header__left">
      <div class="header__logo">
        <div class="icon icon-24 icon__menu"></div>
        <img
          class="img__logo"
          src="../../../assets/img/logoCukCuk.png"
          alt="logo"
        />
      </div>
      <div class="table__text--search combobox">
        <input type="text" readonly class="input__search combox__input" placeholder="Chuỗi nhà hàng" />
        <div class="icon icon-16 icon__down icon__dropdown--header"></div>
      </div>
    </div>
    <div class="header__right">
      <div class="version title__body">R58.0</div>
      <div class="table__text--search combobox">
        <input type="text" readonly class="input__search combox__input" placeholder="Tiếng Việt (Tiếng Việt)" />
        <div class="icon icon-16 icon__down icon__dropdown--header"></div>
      </div>
      <i class="fa-solid fa-earth-americas icon__earth"></i>
      <i class="fa-solid fa-square-phone icon__phone"></i>
      <div class="icon icon-32 icon__avatar"></div>
      <div class="title__body">Vương Thị Hải Yến</div>
      <div class="icon icon-24 icon__down icon__dropdown--header"></div>
    </div>
  </div>
</template>
<style scoped>
@import url(header.css);
</style>