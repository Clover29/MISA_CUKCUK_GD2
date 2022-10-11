<template>
      <div class="alert">
    <div class="alert__content">
      <div class="alert__content--tittle" v-show="alertType == success">
       <div class="icon icon-24 icon__success"></div>
      <span class="alert__tittle success">Thành công! </span>
      </div>
      <div class="alert__content--tittle" v-show="alertType == error">
       <div class="icon icon-24 icon__error"></div>
      <span class="alert__tittle error">Thất bại! </span>
      </div>
      <div class="alert__text">{{msg}}</div>
    </div>
      <div class="icon icon-16 icon__alert--close" @click="btnOnCloseClick"></div>
    </div>
</template>
<style scoped>
@import url(alert.css);
</style>
import  from ''
<script>
import { MISAEnum } from "@/js/Enum";

export default {
  name: "TheAlert",
  props: ["showAlert", "msg", "alertType"],
  data(){
    return{
      success: MISAEnum.Status.Success,
      error: MISAEnum.Status.Error
    }
  },
  methods: {
    /**
     *Đóng alert khi click vào nút close
     * AUTHOR: YENVTH
     * CreatedDate: 08/09/2022
     */
    btnOnCloseClick(){
      this.showAlert(false);
    }
  }
  }
</script>