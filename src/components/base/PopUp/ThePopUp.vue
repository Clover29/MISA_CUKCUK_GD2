<template>
  <div class="dialog">
    <div class="dialog__content" style="width: 400px">
      <div class="dialog__header">
        <div class="form__title">CUKCUK - Quản lý nhà hàng</div>
        <div class="dialog__header--rigt">
          <i
            class="fa-solid fa-circle-xmark icon__close"
            @click="btnCloseOnClick"
          ></i>
        </div>
      </div>
      <div class="dialog__body">
        <div class="dialog__body--data">  <div class="icon__message">
          <div
            class="icon icon-48"
            :class="{
              icon__alert: PopUpType == error,
              icon__question: PopUpType == confirm || PopUpType == alert,
            }"
          ></div>
        </div>
        <div class="pop-up__message">
          <div v-for="item in message" :key="item" class="pop-up__text">
            {{ item.msg }}
          </div>
        </div></div>
        <div class="popup__footer">
          <button ref="firstButton" v-if="PopUpType == error" class="btn col__height--28" @click="btnCloseOnClick">
             Đóng
            </button>
          <div class="footer__right" v-else>
            <button  ref="firstButton" class="btn col__height--28" style="margin-right: 8px" @click="onBtnYesClick"  @keydown.shift.tab.prevent.exact="repeatTabIndex('cancel')">Có</button>
            <button ref="secondButton" class="btn col__height--28" style="margin-right: 8px" @click="onBtnNoClick">Không</button>
            <button ref="cancel"  v-if="PopUpType == confirm" class="btn col__height--28" @click="btnCloseOnClick"   @keydown.tab.prevent="repeatTabIndex('firstButton')">
             Hủy
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped>
@import url(popup.css);
</style>
  <script>
import { MISAEnum } from '@/js/Enum';
import { Constant } from '@/js/Constant';
export default {
  name:"ThePopUp",
  props:["message", "PopUpType","closePopUp"],
  data(){
    return{
      confirm: MISAEnum.PopUpType.Confirm, // pop up confirm
      error: MISAEnum.PopUpType.Error, // pop up báo lỗi
      alert: MISAEnum.PopUpType.Alert // pop up cảnh báo
    }
  },
  methods:{
      /**
     * quay lại tab index
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
     repeatTabIndex(value) {
      this.$refs[value].focus();
    },
     /**
     *Hàm đóng popup
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    btnCloseOnClick(){
      
      if(this.PopUpType == this.error){
         this.$emit(Constant.EMIT_FOCUS);
      }
        this.closePopUp(false); 
    },
     /**
     *Hàm bắt sự kiện khi click button "Yes"
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onBtnYesClick(){
      this.closePopUp(false);
     this.$emit(Constant.EMIT_YES_CLICK);
    },
     /**
     *Hàm bắt sự kiện khi click  "No"
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onBtnNoClick(){
      this.closePopUp(false);
      this.$emit(Constant.EMIT_NO_CLICK);
    }
  },
  mounted(){
    if(this.$refs.firstButton) this.$refs.firstButton.focus();
  }
};
</script>