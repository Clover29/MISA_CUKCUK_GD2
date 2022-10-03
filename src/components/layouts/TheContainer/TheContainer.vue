<template>
  <div class="container">
    <div class="content__tittle">
      <div class="title">Nguyên vật liệu</div>
      <button class="btn">
        <i class="fa-solid fa-bullhorn icon__horn"></i>Phản hồi
      </button>
    </div>
    <div class="content__table">
      <div class="table__option">
        <div class="label color__black" @click="showForm(true)">
          <i class="fa-solid fa-file-circle-plus icon__add"></i>Thêm
        </div>
        <div class="label color__black">
          <i class="fa-regular fa-copy icon__duplicate"></i>Nhân Bản
        </div>
        <div class="label color__black">
          <i class="fa-solid fa-pen-to-square icon__add"></i>Sửa
        </div>
        <div class="label color__black">
          <i class="fa-solid fa-xmark icon__delete"></i>Xóa
        </div>
        <div class="label color__black">
          <i class="fa-solid fa-arrows-rotate icon__add"></i>Nạp
        </div>
      </div>
      <table class="table__data">
        <tr>
          <th>
            Mã nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
              ref="MaterialCode"
              :isButtonShow="true"
              :isSelectShow="false"
              :option_class="'fillter__position'"
              :option="fillterOption"
              :optionType="fillterType"
              :optionID="fillterOption[0].id"
              @fillter = "getFillterOption('MaterialCode')"
            />
              <input v-model="MaterialCode" type="text" class="input__search"/>
            </div>
          </th>
          <th>
            Tên nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
              ref = "MaterialName"
              :isButtonShow="true"
              :isSelectShow="false"
              :option_class="'fillter__position'"
              :option="fillterOption"
              :optionType="fillterType"
              :optionID="fillterOption[0].id"
              @fillter = "getFillterOption"
            />
              <input type="text" class="input__search" v-model="MaterialName"/>
            </div>
          </th>
          <th>
            Tính chất
            <div class="table__text--search">
              <SelectionBox
              ref = "Feature"
              :isButtonShow="true"
              :isSelectShow="false"
              :option_class="'fillter__position'"
              :option="fillterOption"
              :optionType="fillterType"
              :optionID="fillterOption[0].id"
              @fillter = "getFillterOption"
            />
              <input type="text" class="input__search" v-model="Feature" />
            </div>
          </th>
          <th>
            ĐVT
            <div class="table__text--search">
              <SelectionBox
              ref="UnitName"
              :isButtonShow="true"
              :isSelectShow="false"
              :option_class="'fillter__position'"
              :option="fillterOption"
              :optionType="fillterType"
              :optionID="fillterOption[0].id"
              @fillter = "getFillterOption"
            />
              <input type="text" class="input__search" v-model="UnitName" />
            </div>
          </th>
          <th>
            Nhóm nguyên vật liệu
            <div class="table__text--search">
              <SelectionBox
              ref="CategoryName"
              :isButtonShow="true"
              :isSelectShow="false"
              :option_class="'fillter__position'"
              :option="fillterOption"
              :optionType="fillterType"
              :optionID="fillterOption[0].id"
              @fillter = "getFillterOption"
            />
              <input type="text" class="input__search" v-model="CategoryName" />
            </div>
          </th>
          <th>
            Ghi chú
            <div class="table__text--search">
              <SelectionBox
              ref="Description"
              :isButtonShow="true"
              :isSelectShow="false"
              :option_class="'fillter__position'"
              :option="fillterOption"
              :optionType="fillterType"
              :optionID="fillterOption[0].id"
              @fillter = "getFillterOption"
            />
              <input type="text" class="input__search" v-model="Description" />
            </div>
          </th>
          <th>
            Ngừng theo dõi
            <SelectionBox
            :isSelectShow="true"
              :div_class="'table__header--box'"
              :input_class="'select__input col_width_100'"
              :option="statusOption"
              :optionType="statusType"
              :value="statusOption[0].text"
              :optionID="statusOption[0].id"
              @Status="setStatus"
            />
          </th>
        </tr>
        <tr v-for="item in materialList" :key="item.materialID">
          <td>{{ item.materialCode }}</td>
          <td>{{ item.materialName }}</td>
          <td>{{ item.feature }}</td>
          <td>{{ item.unitName }}</td>
          <td>{{ item.categoryName }}</td>
          <td>{{ item.description }}</td>
          <td class="checkbox__col">
            <input
              type="checkbox"
              :checked="item.status == 2"
              :value="item.status"
            />
          </td>
        </tr>
      </table>
    </div>
    <MaterialPaging
      :totalRecord="totalPageRecord"
      @LoadData="loadData"
      :totalPage="totalPage"
      :materialFillter="materialFillter"
    />
    <MaterialForm v-if="isShowForm" :showForm="showForm" />
  </div>
</template>
<style scoped>
@import url(container.css);
</style>
<script>
import axios from "axios";
import { MISAEnum } from "../../../js/Enum.js";
import { Resources } from "../../../js/Resources.js";
import MaterialForm from "../../base/Form/MaterialForm.vue";
import MaterialPaging from "../../base/Paging/MaterialPaging.vue";
import SelectionBox from "../../base/SelectionBox/SelectionBox.vue";
export default {
  name: "TheContainer",
  components: { MaterialForm, MaterialPaging, SelectionBox },
  data() {
    return {
      isShowForm: false, //hiển thị form thông tin nguyên liệu,
      MaterialCode: "",
        MaterialName: "",
        Feature: "",
        UnitName: "",
        CategoryName: "",
        Description: "",
      materialFillter: {
        MaterialCode: "",
        MaterialName: "",
        Feature: "",
        UnitName: "",
        CategoryName: "",
        Description: "",
        Status: 1,
      }, //danh sách các thuộc tính lọc nguyên vật liệu
      materialList: [], //danh sách nguyên vật liệu,
      totalPageRecord: 0, // tổng số bản ghi được tìm thấy
      totalPage: 0, // tổng số trang
      statusOption: [
        { id: 1,value: MISAEnum.StatusOption.Using.value, text:MISAEnum.StatusOption.Using.text},
        { id: 2,value: MISAEnum.StatusOption.NotUsing.value, text:MISAEnum.StatusOption.NotUsing.text},
      ],
      statusType: MISAEnum.OptionType.Status,
      fillterOption: [
        { id: 1, value:MISAEnum.FillterOption.Contain.value, text:MISAEnum.FillterOption.Contain.text  },
        { id: 2, value: MISAEnum.FillterOption.Equal.value, text: MISAEnum.FillterOption.Equal.text },
        { id: 3, value: MISAEnum.FillterOption.StartWidth.value, text: MISAEnum.FillterOption.StartWidth.text},
        { id: 4, value:MISAEnum.FillterOption.EndWidth.value, text: MISAEnum.FillterOption.EndWidth.text },
        { id: 5, value: MISAEnum.FillterOption.NotContain.value, text: MISAEnum.FillterOption.NotContain.text },
      ],
      fillterType: MISAEnum.OptionType.Fillter,
      fillterText:MISAEnum.FillterOption.Contain.value,
    };
  },
  methods: {
    /**
     *Hàm mở form
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    showForm(isShow) {
      this.isShowForm = isShow;
    },
    /**
     *Hàm load danh sách nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    loadData(pageSize, pageNumber, materialFillter) {
      axios
        .get(
          Resources.DOMAIN +
            Resources.API_VER +
            Resources.MATERIAL_PATH +
            `/Fillter?MaterialCode=${materialFillter.MaterialCode}&MaterialName=${materialFillter.MaterialName}&
            Feature=${materialFillter.Feature}&UnitName=${materialFillter.UnitName}&CategoryName=${materialFillter.CategoryName}&
            Description=${materialFillter.Description}&Status=${materialFillter.Status}&pageSize=${pageSize}&pageNumber=${pageNumber}`
        )
        .then((response) => {
          if (response) {
            this.materialList = response.data.data;
            this.totalPage = response.data.totalPage;
            this.totalPageRecord = response.data.totalRecord;
            this.currentPage = response.data.currentPage;
            // this.loading = false;
            // this.setPagingValue();
            // this.isData = response.data.data.length > 0 ? true : false;
            // this.showPaging = response.data.data.length > 0 ? true : false;
          }
        })
        .catch((error) => {
          console.log(error.response);
          // this.errorMsg = [];
          // this.popUpType = MISAEnum.PopUpType.Alert;
          // this.showPopUp = true;
          // this.errorMsg.push({
          //   msg: Resources.UNDEFINED_ERROR,
          //   tittle: MISAEnum.Status.Error,
          // });
        });
    },
    setStatus(data){
      this.materialFillter.Status = data;
      this.loadData(10,1,this.materialFillter);
    },
    getFillterOption(value){
      if(this.$refs[value].fillterText != "+"){
         this.materialFillter[value] = this.$refs[value].fillterText + this[value];
      }
    else{
      this.materialFillter[value] = "%2B" + this[value];
    }
     this.loadData(10,1,this.materialFillter);
    }
  },
  created() {
    this.loadData(10, 1, this.materialFillter);
  },
};
</script>
