<template>
  <div class="autocomplte" :class="div_class">
    <input
      class="form__text"
      :class="text_class"
      type="text"
      ref="input"
      v-model = "value"
      @input="onEditName"
      @blur="validate(false)"
      @keydown="keyMonitor($event)"
    />
    <OnClickOutside @trigger="onClickOutside">
       <div
      class="icon icon-16 icon__dropdown"
      :class="dropdown__icon"
      @click="showDropDown"
    ></div >
    </OnClickOutside>
    <i
      class="fa-solid fa-plus icon__add position__plus"
      :class="plus__icon"
      @click="btnAddOnClick"
    ></i>
    <div class="combobox__option unit_position" ref="scrollContainer" :class="width" v-show="isShow">
      <div
        v-for="item in filteredOptions"
        :key="item.Id"
        class="combobox__option--row"
        :class="{ combobox__highlight: ID == item.Id }"
        @click="onClickOption(item)"
      >
        {{ item.Name }}
      </div>
    </div>
  </div>
</template>
  <style scoped>
@import url(combobox.css);
</style>
   <script>
   import { Resources } from "@/js/Resources";
import { OnClickOutside } from "@vueuse/components";
import { MISAEnum } from '@/js/Enum';
export default {
  name: "ComboboxVue",
  components: {OnClickOutside},
  props: 
  ["div_class",
    "width",
    "plus__icon",
    "dropdown__icon",
    "text_class",
    "UnitList",
    "StockList",
    "ID",
    "Name",
  ],
  data() {
    return {
      isShow: false, // hiển thị option
      isDropdownClick:false, // check button dropdown có đc click hay k
      index:0, // chỉ số của item được lựa chọn
      value:this.Name ? this.Name : "", // giá trị của input
    }
  },
  watch:{
     /**
     *set giá trị của value tương ứng với prop Name
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    Name:function(value){
      this.value = value ? value : ""; 
      this.validate(false);
    },
     /**
     *update prop Name và id khi index thay đổi
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    index() {
      try {
        this.$emit(Resources.EMIT_UPDATE_NAME,this.$refs.input.value); 
        this.$emit(Resources.EMIT_UPDATE_ID,this.filteredOptions[this.index].Id);
      } catch (error) {
        console.log(error);
      }
    },
  },
  computed: {
     /**
     *set giá trị của optionData tương ứng với loại form cần thực hiện
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    optionData() {
      var optionData = [];
      if (this.UnitList) {
        for (let index = 0; index < this.UnitList.length; index++) {
          optionData.push({
            Id: this.UnitList[index].unitID,
            Name: this.UnitList[index].unitName,
          });
        }
      }
      if( this.StockList){
        for (let index = 0; index < this.StockList.length; index++) {
          optionData.push({
            Id: this.StockList[index].stockID,
            Name: this.StockList[index].stockName,
          });
        }
      }
      return optionData;
    },
     /**
     *hàm thực hiện chức năng autocomplte
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    filteredOptions() {
      if (!this.ID || this.isDropdownClick
      ) {
        return this.optionData;
      } else {
        const filtered = [];
        const regOption = new RegExp(this.value, "ig");
           for (const option of this.optionData) {
          if (option.Name.match(regOption) ) {
            filtered.push(option);
          }
        }
        return filtered;
      }
    },
  },
  methods: {
     /**
     *Hàm ẩn hiện option khi click button dropdown
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    showDropDown() {
      this.isShow = !this.isShow;
    },
     /**
     *Hàm set giá trị khi click vào item tương ứng
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onClickOption(value) { 
      try {
      this.isDropdownClick = true;
      this.$emit(Resources.EMIT_UPDATE_NAME,value.Name );
      this.$emit(Resources.EMIT_UPDATE_ID,value.Id );
      this.$emit(Resources.EMIT_VALIDATE_UNIT);
      this.value = value.Name;
      this.isShow = false;
      } catch (error) {
        console.log(error);
      }
     
    },
     /**
     * tắt dropdown khi click ra các vùng bên ngoài
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
     onClickOutside() {
      try {
        this.isShow = false;
      } catch (error) {
        console.log(error);
      }
    },
     /**
     *hiện option khi edit input
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onEditName() {
      try {
        this.isShow = true;
        this.isDropdownClick = false;
        this.index = 0;
        this.$emit(Resources.EMIT_UPDATE_ID,this.filteredOptions[0].Id);
      } catch (error) {
        console.log(error);
      }
    },
     /**
     *validate đơn vị
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    validate(isSaveClick){
      if(!this.text_class&&(this.Name!=""|| isSaveClick)){
        if(this.$refs.input && this.value==""){
          this.$refs.input.classList.add(Resources.BORDER_CLASS);
        }else{
          this.$refs.input.classList.remove(Resources.BORDER_CLASS);
        }
      }
    },
     /**
     *mở form add khi click vào button cộng
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    btnAddOnClick(){
      this.isDropdownClick = true;
      this.$emit(Resources.EMIT_ADD);
    },
     /**
     * set vị trí của thanh cuộn khi kéo lên hoặc kéo xuống
     * AUTHOR: YENVTH
     * CreatedDate: 08/08/2022
     */
     setScroll() {
      try {
        if (this.$refs.scrollContainer) {
          this.$refs.scrollContainer.scrollTop = 25 * this.index;
        }
      } catch (error) {
        console.log(error);
      }
    },
     /**
     *set index tương ứng với các phím tương ứng
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    keyMonitor(event) {
      try {
        if (this.filteredOptions) {
          switch (event.key) {
            case MISAEnum.KeyBoard.Enter:
              this.onClickOption(this.filteredOptions[this.index]);
              break;
            case MISAEnum.KeyBoard.ArrowDown: 
            if(this.isShow) this.index =
                this.index >= this.filteredOptions.length - 1
                  ? 0
                  : this.index + 1;
              this.isShow = true;
              this.setScroll();
              break;
            case MISAEnum.KeyBoard.ArrowUp:
                if(this.isShow) this.index =
                this.index <= 0
                  ? this.filteredOptions.length - 1
                  : this.index - 1;
              this.isShow = true;
              this.setScroll();
              break;
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    
  },
  mounted(){
    this.$refs.input.focus();
  }
  
};
</script>